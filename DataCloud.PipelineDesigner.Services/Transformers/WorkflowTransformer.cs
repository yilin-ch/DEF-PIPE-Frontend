﻿using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCloud.PipelineDesigner.Services
{
    public class WorkflowTransformer
    {

        public List<Workflow> GenerateWorkflows(List<Workflow> workflows, List<Canvas> canvas)
        {

            if (canvas.Count == 0)
            {
                return workflows;
            }

            var newCanvas = new Canvas();
            var currentCanvas = canvas[0];
            canvas.RemoveAt(0);

            foreach (var item in currentCanvas.Elements)
            {
                if (item.Type == CanvasElementType.Shape)
                {
                    var canvasShape = JsonConvert.DeserializeObject<CanvasShape>(JsonConvert.SerializeObject(item));
                    newCanvas.Elements.Add(canvasShape);
                    if (canvasShape.Elements.Count > 0)
                        canvas.Add(JsonConvert.DeserializeObject<Canvas>(JsonConvert.SerializeObject(item)));
                }
                else
                    newCanvas.Elements.Add(JsonConvert.DeserializeObject<CanvasConnector>(JsonConvert.SerializeObject(item)));
            }
            var workflow = GenerateWorkflow(newCanvas);
            workflow.Name = currentCanvas.Name;
            workflows.Add(workflow);

            return GenerateWorkflows(workflows, canvas);
        }
        public Workflow GenerateWorkflow(Canvas canvas)
        {

            Workflow workflow = new Workflow();

            var startElement = canvas.Elements
                .Where(e => e.Type == CanvasElementType.Shape)
                .Select(e => e as CanvasShape)
                .First(e => e.TemplateId.ToLower() == Constants.BuiltInTemplateIDs.Start.ToLower());

            workflow.Parameters = new Dictionary<string, string>(startElement.PropertiesDict);

            MapNextShape(workflow, workflow.Elements, canvas, startElement);

            return workflow;
        }

        private void MapNextShape(Workflow workflow, List<WorkflowElement> workflowElements, Canvas canvas, CanvasShape currentShape)
        {


            var connectors = FindOutputConnectorsFromShape(canvas, currentShape);
            foreach (var connector in connectors)
            {
                var nextShape = canvas.Elements?.First(e => e.ID == connector.DestShapeId) as CanvasShape;
                if (nextShape.TemplateId?.ToLower() == Constants.BuiltInTemplateIDs.End.ToString().ToLower())
                {
                    continue;
                }
                else if (IsDataset(nextShape))
                {
                    workflow.DataSets.Add(MapToWorkflowDataset(nextShape));
                    MapNextShape(workflow, workflowElements, canvas, nextShape);
                }
                else if (nextShape.TemplateId?.ToLower() == Constants.BuiltInTemplateIDs.If.ToString().ToLower())
                {
                    var workflowControl = MapToWorkflowSwithcControl(workflow, canvas, nextShape);
                    workflowElements.Add(workflowControl);
                }
                else
                {
                    workflowElements.Add(MapToWorkflowAction(canvas, nextShape));
                    MapNextShape(workflow, workflowElements, canvas, nextShape);
                }
            }
        }

        private List<CanvasConnector> FindOutputConnectorsFromShape(Canvas canvas, CanvasShape canvasShape)
        {
            return canvas.Elements
                .Where(e => e.Type == CanvasElementType.Connector)
                .Select(e => e as CanvasConnector)
                .Where(e => e.SourceShapeId == canvasShape.ID)
                .ToList();
        }


        private WorkflowAction MapToWorkflowAction(Canvas canvas, CanvasShape canvasShape)
        {
            WorkflowAction workflowAction = new WorkflowAction(canvasShape.Parameters);

            workflowAction.ID = canvasShape.ID;
            workflowAction.Title = canvasShape.Name;
            workflowAction.InputDataSetId = FindInputDataset(canvas, canvasShape);
            workflowAction.OutputDataSetId = FindOutputDataset(canvas, canvasShape);

            return workflowAction;
        }

        private Guid? FindInputDataset(Canvas canvas, CanvasShape canvasShape)
        {
            var connectors = canvas.Elements
                .Where(e => e.Type == CanvasElementType.Connector)
                .Select(e => e as CanvasConnector)
                .Where(e => e.DestShapeId == canvasShape.ID)
                .ToList();

            var inputDataset = canvas.Elements
                .Where(e => e.Type == CanvasElementType.Shape)
                .Select(e => e as CanvasShape)
                .FirstOrDefault(e => connectors.Any(c => c.SourceShapeId == e.ID) && IsDataset(e));

            return inputDataset != null ? new Guid(inputDataset.ID) : (Guid?)null;
        }

        private Guid? FindOutputDataset(Canvas canvas, CanvasShape canvasShape)
        {
            var connectors = canvas.Elements
                    .Where(e => e.Type == CanvasElementType.Connector)
                    .Select(e => e as CanvasConnector)
                    .Where(e => e.SourceShapeId == canvasShape.ID)
                    .ToList();

            var outputDataset = canvas.Elements
                .Where(e => e.Type == CanvasElementType.Shape)
                .Select(e => e as CanvasShape)
                .FirstOrDefault(e => connectors.Any(c => c.DestShapeId == e.ID) && IsDataset(e));

            return outputDataset != null ? new Guid(outputDataset.ID) : (Guid?)null;
        }

        public WorkflowDataSet MapToWorkflowDataset(CanvasShape canvasShape)
        {
            WorkflowDataSet workflowDataset = new WorkflowDataSet();

            workflowDataset.Id = new Guid(canvasShape.ID);
            workflowDataset.Parameters = new Dictionary<string, string>(canvasShape.PropertiesDict);

            return workflowDataset;
        }

        public WorkflowSwitchControl MapToWorkflowSwithcControl(Workflow workflow, Canvas canvas, CanvasShape canvasShape)
        {
            if (canvasShape.TemplateId.ToLower() != Constants.BuiltInTemplateIDs.If.ToString().ToLower())
                return null;

            WorkflowSwitchControl workflowControl = new WorkflowSwitchControl();
            workflowControl.ID = canvasShape.ID;
            workflowControl.InputDataSetId = FindInputDataset(canvas, canvasShape);
            workflowControl.SwitchCases.Add(canvasShape.PropertiesDict[Constants.BuiltInCanvasShapeProperties.Condition], new List<WorkflowElement>());
            workflowControl.Elements = new List<WorkflowElement>();

            var nextOutputConnectors = FindOutputConnectorsFromShape(canvas, canvasShape);
            if (nextOutputConnectors.Count > 0)
            {
                var firstCaseShape = canvas.Elements.First(e => e.ID == nextOutputConnectors[0].DestShapeId) as CanvasShape;
                var firstCaseAction = MapToWorkflowAction(canvas, firstCaseShape);
                firstCaseAction.InputDataSetId = workflowControl.InputDataSetId;

                workflowControl.SwitchCases[canvasShape.PropertiesDict[Constants.BuiltInCanvasShapeProperties.Condition]].Add(firstCaseAction);
                MapNextShape(workflow, workflowControl.SwitchCases[canvasShape.PropertiesDict[Constants.BuiltInCanvasShapeProperties.Condition]], canvas, firstCaseShape);
            }

            if (nextOutputConnectors.Count > 1)
            {
                var secondCaseShape = canvas.Elements.First(e => e.ID == nextOutputConnectors[1].DestShapeId) as CanvasShape;
                var secondCaseAction = MapToWorkflowAction(canvas, secondCaseShape);
                secondCaseAction.InputDataSetId = workflowControl.InputDataSetId;

                workflowControl.Elements.Add(secondCaseAction);
                MapNextShape(workflow, workflowControl.Elements, canvas, secondCaseShape);
            }

            return workflowControl;
        }

        private bool IsDataset(CanvasShape canvasShape)
        {
            return canvasShape.Shape == Constants.BuiltyInCanvasShape.Database;
        }

        public Dsl GenerateDsl(List<Workflow> workflows, List<CanvasProvider> providers)
        {
            Dsl dsl = new Dsl();

            dsl.Pipeline = GeneratePipeline(workflows[0]);

            var subPipelines = new List<Pipeline>();
            foreach (var item in workflows.Skip(1))
                subPipelines.Add(GeneratePipeline(item, true));

            dsl.SubPipelines = subPipelines.ToArray();

            dsl.ResourceProvider = providers?.Select(p => new ResourceProvider
            {
                Provider = p.provider,
                MappingLocation = p.mappingLocation,
                Name = p.name,
                ProviderLocation = p.providerLocation,

            }).ToArray();

            return dsl;
        }

        public Pipeline GeneratePipeline(Workflow workflow, bool isSubPipline = false)
        {
            Pipeline pipeline = new Pipeline();
            pipeline.Name = workflow.Name;
            List<Step> steps = new List<Step>();

            foreach (WorkflowElement element in workflow.Elements)
            {
                Step step = new Step();
                step.StepType = (element as WorkflowAction).Parameters?.StepType;
                step.Type = isSubPipline ? "subPipeline" : "step";
                if (element.ElementType == WorkflowElementType.Action)
                    step.Name = (element as WorkflowAction).Title;
                step.Implementation = new Implementation
                {
                    ImageName = (element as WorkflowAction).Parameters?.Image,
                    Type = (element as WorkflowAction).Parameters?.StepImplementation,
                };
                step.ResourceProvider = (element as WorkflowAction).Parameters?.ResourceProvider;
                step.EnvParams = (element as WorkflowAction).Parameters?.EnvironmentParameters?.ToDictionary((ep) => ep.Key, (ep) => ep.Value);


                // Block to improve...
                var hard = (JArray)(element as WorkflowAction).Parameters?.ExecutionRequirement?["hardRequirements"];



                step.ExecRequirements = new ExecutionRequirements[2];

                step.ExecRequirements[0] = new ExecutionRequirements
                {
                    Type = "hardRequirements",
                    SubTypeRequirements = ((IEnumerable<dynamic>)hard)?.Select(e => {

                        Dictionary<string, string> r = ((IEnumerable<KeyValuePair<string, JToken>>)e)
                                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

                        Dictionary<string, string> r2 = ((IEnumerable<KeyValuePair<string, JToken>>)e).Skip(1)
                                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());


                        return new RequirementsSubType { SubType = r.First().Value, Requirements = r2 };
                    }).ToArray()
                };

                steps.Add(step);

            }
            pipeline.Steps = steps.ToArray();

            return pipeline;
        }
    }
}
