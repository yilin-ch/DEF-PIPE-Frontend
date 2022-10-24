using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Repositories.Models;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataCloud.PipelineDesigner.Services.Constants;
using Newtonsoft.Json.Linq;

namespace DataCloud.PipelineDesigner.Services
{
    public class CanvasService


    {

        public static CanvasShapeTemplate TransformDslToCanvas(Dsl dsl)
        {
            CanvasShapeTemplate pipeline = TransformPipelineToCanvas(dsl.Pipeline, dsl.SubPipelines);


            return pipeline;
        }

        private static CanvasShapeTemplate TransformPipelineToCanvas(Pipeline pipeline, Pipeline[] subPipelines)
        {
            CanvasShapeTemplate template = new CanvasShapeTemplate(pipeline.Name, "", "Imported");
            template.Properties = new List<CanvasElementProperty>();
            var startGUID = Guid.NewGuid().ToString();
            var endGUID = Guid.NewGuid().ToString();

            var start = new CanvasShape
            {
                ID = startGUID,
                TemplateId = BuiltInTemplateIDs.Start.ToString(),
                Name = "Start",

                Shape = BuiltyInCanvasShape.Rectangle,

                ConnectionPoints = new List<CanvasShapeConnectionPoint>() {
                    new CanvasShapeConnectionPoint { Id = "2", Position = new CanvasPosition { X = 25, Y = 50 }, Type = CanvasConnectionPointType.Output }
                },
                Position = new CanvasPosition { X = 25, Y = 100 },
                Type = CanvasElementType.Shape,
                Width = 50,
                Height = 50,
            };

            template.Elements.Add(start);


            var lastShape = start;


            foreach (Step step in pipeline.Steps)
            {

                var guid = Guid.NewGuid().ToString();

                var shape = CreateStepShape(guid, step, lastShape, subPipelines);


                template.Elements.Add(shape);

                template.Elements.Add(
                    new CanvasConnector
                    {
                        ID = Guid.NewGuid().ToString(),
                        SourceShapeId = lastShape.ID,
                        SourceConnectionPointId = "2",
                        DestShapeId = guid,
                        DestConnectionPointId = "1",
                        Type = CanvasElementType.Connector,
                    });

                lastShape = shape;
            }

            

            var end = new CanvasShape
            {
                ID = endGUID,
                TemplateId = BuiltInTemplateIDs.End.ToString(),
                Name = "End",
                Shape = BuiltyInCanvasShape.Rectangle,
                ConnectionPoints = new List<CanvasShapeConnectionPoint>() {
                    new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 25, Y = 50 }, Type= CanvasConnectionPointType.Input }
                },

                Position = NextPositon(lastShape),
                Type = CanvasElementType.Shape,
                Width = 50,
                Height = 50
            };

            template.Elements.Add(end);

            template.Elements.Add(
                new CanvasConnector
                {
                    ID = Guid.NewGuid().ToString(),
                    SourceShapeId = lastShape.ID,
                    SourceConnectionPointId = "2",
                    DestShapeId = endGUID,
                    DestConnectionPointId = "1",
                    Type = CanvasElementType.Connector,
                });

            return template;
        }
        private static CanvasShape CreateStepShape(String guid, Step step, CanvasShape lastShape, Pipeline[] subPipelines)
        {
            var subPipeline = subPipelines.FirstOrDefault(x => step.Name.Contains(x.Name));

            if (subPipeline == null)
            {
                return new CanvasShape
                {
                    ID = guid,
                    Name = step.Name,
                    Shape = BuiltyInCanvasShape.Rectangle,
                    ConnectionPoints = new List<CanvasShapeConnectionPoint>() {
                        new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 0, Y = 100 }, Type= CanvasConnectionPointType.Input },
                        new CanvasShapeConnectionPoint { Id = "2", Position = new CanvasPosition { X = 300, Y = 100 }, Type = CanvasConnectionPointType.Output }
                    },
                    Position = NextPositon(lastShape),
                    Type = CanvasElementType.Shape,
                    Width = 300,
                    Height = 200,
                    Parameters = new CanvasParameters
                    {
                        Image = step.Implementation.ImageName,
                        ResourceProvider = step.ResourceProvider,
                        StepType = step.StepType,
                        StepImplementation = step.Implementation.Type,
                        EnvironmentParameters = step.EnvParams?.Select(e => new EnvironmentParameter { Key = e.Key, Value = e.Value }).ToList(),
                        ExecutionRequirement = GenerateExecutionRequirement(step.ExecRequirements)
                    }
                };
            }
            else
            {
                var nestedCanvas = TransformPipelineToCanvas(subPipeline, subPipelines);
                return new CanvasShape
                {
                    ID = guid,
                    Name = subPipeline.Name,
                    Shape = BuiltyInCanvasShape.Container,
                    ConnectionPoints = new List<CanvasShapeConnectionPoint>() {
                        new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 0, Y = 100 }, Type = CanvasConnectionPointType.Input },
                        new CanvasShapeConnectionPoint { Id = "2", Position = new CanvasPosition { X = 300, Y = 100 }, Type = CanvasConnectionPointType.Output }
                    },
                    Position = NextPositon(lastShape),
                    Type = CanvasElementType.Shape,
                    Width = 300,
                    Height = 200,
                    Elements = nestedCanvas.Elements,
                };
            }


            
        }

        private static CanvasPosition NextPositon(CanvasShape cs)
        {
            var x = cs.Position.X + cs.Width + 30;
            var y = cs.Position.Y;

            CanvasPosition nextPositon = new CanvasPosition { X = x, Y = y};

            return nextPositon;
        }

        private static dynamic GenerateExecutionRequirement(ExecutionRequirements[] e)
        {

            dynamic requ = new JObject();
            dynamic requs = new JArray();

            
            foreach (RequirementsSubType requirement in e[0].SubTypeRequirements)
            {
                dynamic r = new JObject();

                r.reqType = requirement.SubType;

                foreach (var kv in requirement.Requirements)
                {
                    double number;

                    var isDouble = Double.TryParse(kv.Value, out number);

                    if (!isDouble)
                    {
                        r[kv.Key] = kv.Value;
                    }
                    else
                    {
                        r[kv.Key] = number;
                    }
                   
                }

                requs.Add(r);
            }

            requ.hardRequirements = requs;


            return requ;
        }



    }
}
