using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

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
                    {
                        canvas.Add(JsonConvert.DeserializeObject<Canvas>(JsonConvert.SerializeObject(item)));
                        Console.WriteLine(canvasShape.Loop);
                        Console.WriteLine(canvasShape.LoopCondition);
                    }
                        
                }
                else
                    newCanvas.Elements.Add(JsonConvert.DeserializeObject<CanvasConnector>(JsonConvert.SerializeObject(item)));
            }
            var workflow = GenerateWorkflow(newCanvas);
            workflow.Name = currentCanvas.Name;
            workflow.Loop = currentCanvas.Loop;
            workflow.LoopCondition = currentCanvas.LoopCondition;
            workflows.Add(workflow);
            Console.WriteLine("workflow" + workflow.Loop);
            Console.WriteLine("workflow" + workflow.LoopCondition);

            return GenerateWorkflows(workflows, canvas);
        }
        public Workflow GenerateWorkflow(Canvas canvas)
        {

            Workflow workflow = new Workflow();

            // Change Start_Name to Start later
            var startElement = canvas.Elements
                .Where(e => e.Type == CanvasElementType.Shape)
                .Select(e => e as CanvasShape)
                .First(e => e.Name.ToLower() == Constants.BuiltInTemplateIDs.Start_Name.ToLower());

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
                // Change ENd_Name to End later
                if (nextShape.Name?.ToLower() == Constants.BuiltInTemplateIDs.End_Name.ToString().ToLower())
                {
                    continue;
                }
                else if (IsDataset(nextShape))
                {
                    workflow.DataSets.Add(MapToWorkflowDataset(nextShape));
                    MapNextShape(workflow, workflowElements, canvas, nextShape);
                }
                // Change Type_Name to Type later
                else if (nextShape.Conditional?.ToLower() == Constants.BuiltInTemplateIDs.Type_Name.ToString().ToLower())
                {
                    var workflowControl = MapToWorkflowSwitchControlforYaml(workflow, canvas, nextShape);
                    if (currentShape.Name?.ToLower() != Constants.BuiltInTemplateIDs.Start_Name.ToString().ToLower())
                        // Change Start_Name to Start later
                        workflowControl.Dependencies.Add(currentShape.ID);
                    workflowElements.Add(workflowControl);
                }
                else
                {
                    if (workflowElements.Exists(e => e.ID == nextShape.ID))
                    {
                        // Todo: Should the action implemented multiple times if mulitple connectors connects to it? Or just once.
                        WorkflowAction action = workflowElements.First(e => e.ID == nextShape.ID) as WorkflowAction;
                        if (action != null)
                            if (currentShape.Name?.ToLower() != Constants.BuiltInTemplateIDs.Start_Name.ToString().ToLower())
                                // Change Start_Name to Start later
                                action.Dependencies.Add(currentShape.ID);
                    }
                    else
                        workflowElements.Add(MapToWorkflowAction(canvas, nextShape, currentShape));
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

        private WorkflowAction MapToWorkflowAction(Canvas canvas, CanvasShape canvasShape, CanvasShape prevShape)
        {
            WorkflowAction workflowAction = new WorkflowAction(canvasShape.Parameters, canvasShape.ID);

            // Change Start_Name to Start later
            if (prevShape.Name?.ToLower() != Constants.BuiltInTemplateIDs.Start_Name.ToString().ToLower())
                workflowAction.Dependencies.Add(prevShape.ID);
            if (canvasShape.Elements.Count > 0)
                workflowAction.Subpipeline = true;
            workflowAction.ID = canvasShape.ID;
            workflowAction.Title = canvasShape.Name;
            workflowAction.Condition = canvasShape.Condition;
            Console.WriteLine(canvasShape.Parameters.Image);
            workflowAction.InputDataSetId = FindInputDataset(canvas, canvasShape);
            workflowAction.OutputDataSetId = FindOutputDataset(canvas, canvasShape);

            return workflowAction;
        }

        private WorkflowAction MapToWorkflowAction(Canvas canvas, CanvasShape canvasShape)
        {
            WorkflowAction workflowAction = new WorkflowAction(canvasShape.Parameters);

            workflowAction.ID = canvasShape.ID;
            workflowAction.Title = canvasShape.Name;
            workflowAction.Condition = canvasShape.Condition;
            Console.WriteLine(canvasShape.Parameters.Image);
            Console.WriteLine(canvasShape.Parameters.Additional);
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

        public WorkflowSwitchControl MapToWorkflowSwitchControlforYaml(Workflow workflow, Canvas canvas, CanvasShape canvasShape)
        {
            // Change If_Name to If later 
            if (canvasShape.Conditional.ToLower() != Constants.BuiltInTemplateIDs.Type_Name.ToString().ToLower())
                return null;

            WorkflowSwitchControl workflowControl = new WorkflowSwitchControl();
            workflowControl.ID = canvasShape.ID;
            workflowControl.InputDataSetId = FindInputDataset(canvas, canvasShape);
            workflowControl.SwitchCases.Add("first", new List<WorkflowElement>());
            workflowControl.Elements = new List<WorkflowElement>();

            var workflowControlAction = MapToWorkflowAction(canvas, canvasShape);
            workflowControl.WorkflowControlAction = workflowControlAction;

            Console.WriteLine(canvasShape.Condition);
            var nextOutputConnectors = FindOutputConnectorsFromShape(canvas, canvasShape);
            if (nextOutputConnectors.Count > 0)
            {
                var firstCaseShape = canvas.Elements.First(e => e.ID == nextOutputConnectors[0].DestShapeId) as CanvasShape;
                var firstCaseAction = MapToWorkflowAction(canvas, firstCaseShape, canvasShape);
                // Todo: consider consecutive ifs.
                firstCaseAction.InputDataSetId = workflowControl.InputDataSetId;
                
                workflowControl.SwitchCases["first"].Add(firstCaseAction);
                // Todo: change condition based on the connector
                MapNextShape(workflow, workflowControl.SwitchCases["first"], canvas, firstCaseShape);
            }

            if (nextOutputConnectors.Count > 1)
            {
                var secondCaseShape = canvas.Elements.First(e => e.ID == nextOutputConnectors[1].DestShapeId) as CanvasShape;
                var secondCaseAction = MapToWorkflowAction(canvas, secondCaseShape, canvasShape);
                secondCaseAction.InputDataSetId = workflowControl.InputDataSetId;

                workflowControl.SwitchCases.Add("second", new List<WorkflowElement>());
                workflowControl.SwitchCases["second"].Add(secondCaseAction);
                MapNextShape(workflow, workflowControl.SwitchCases["second"], canvas, secondCaseShape);
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

        public ArgoYamlFlow GenerateYaml(List<Workflow> workflows, string name)
        {
            int workflowNum = 0;
            ArgoYamlFlow yaml = new ArgoYamlFlow();

            yaml.Name = name.Trim().Replace(" ", "_").ToLower();
            yaml.Steps = GenerateYamlPipeline(workflows, workflowNum).ToArray();

            Console.WriteLine(yaml.Steps.Length);

            return yaml;
        }

        private List<YamlStep> GenerateYamlPipeline(List<Workflow> workflows, int workflowNum)
        {
            List<YamlStep> steps = new List<YamlStep>();
            Dictionary<YamlStep, int> workflowToStep = new Dictionary<YamlStep, int>();

            foreach (WorkflowElement element in workflows[workflowNum].Elements)
            {
                YamlStep step = new YamlStep();
                if (element.ElementType == WorkflowElementType.Action)
                {
                    step.IsSubpipeline = (element as WorkflowAction).Subpipeline;
                    if (step.IsSubpipeline)
                    {
                        workflowNum++;
                        workflowToStep.Add(step, workflowNum);
                    }

                    FillStepDetails(step, (element as WorkflowAction));
                    if ((element as WorkflowAction).Dependencies != null && (element as WorkflowAction).Dependencies.Count > 0)
                    {
                        step.Dependencies = new HashSet<string>((element as WorkflowAction).Dependencies);
                        // Need to add all dependencies
                        step.Previous = (element as WorkflowAction).Dependencies.First();
                    }

                    steps.Add(step);
                }
                
                if (element.ElementType == WorkflowElementType.Control)
                    if ((element as WorkflowControl).ControlType == WorkflowControlType.Switch)
                    {
                        Console.WriteLine("a");
                        step.Name = "condition";
                        step.ID = (element as WorkflowSwitchControl).ID;
                        step.IsConditional = true;
                        step.conditionPipelines = new Dictionary<string, ArgoYamlFlow>();
                        foreach (var pair in (element as WorkflowSwitchControl).SwitchCases)
                        {
                            Console.WriteLine(pair.Key);
                            ArgoYamlFlow subPipeline = new ArgoYamlFlow();
                            subPipeline.Steps = GenerateConditionPipeline(pair.Value, workflowToStep, workflowNum).ToArray();
                            step.conditionPipelines.Add(pair.Key, subPipeline);
                        }
                        Console.WriteLine("c");
                        if ((element as WorkflowSwitchControl).Dependencies != null && (element as WorkflowSwitchControl).Dependencies.Count > 0)
                        {
                            step.Dependencies = new HashSet<string>((element as WorkflowSwitchControl).Dependencies);
                            // Need to add all dependencies
                            step.Previous = (element as WorkflowSwitchControl).Dependencies.First();
                        }
                        Console.WriteLine("d");

                        YamlStep actionForConditional = new YamlStep();
                        WorkflowAction action = (element as WorkflowSwitchControl).WorkflowControlAction;
                        FillStepDetails(actionForConditional, action);
                        step.ActionForConditional = actionForConditional;

                        steps.Add(step);
                    }
            }

            // Todo: find a more sophisticated way to match each substep with each workflow 
            foreach (var pair in workflowToStep)
            {
                ArgoYamlFlow subPipeline = new ArgoYamlFlow();
                subPipeline.Steps = GenerateYamlPipeline(workflows, pair.Value).ToArray();
                subPipeline.IsLoop = int.Parse(workflows[pair.Value].Loop) == 1;
                if (subPipeline.IsLoop)
                    subPipeline.LoopCondition = workflows[pair.Value].LoopCondition;
                Console.WriteLine("YamlWorkflow " + subPipeline.IsLoop);
                Console.WriteLine("YamlWorkflow" + subPipeline.LoopCondition);
                pair.Key.subPipeline = subPipeline;
            }

            foreach(var step in steps)
            {
                if (step.IsSubpipeline)
                {
                    Console.WriteLine(step.Name);
                    Console.WriteLine(step.subPipeline);
                }
                    
            }

            return steps;
        }

        private List<YamlStep> GenerateConditionPipeline(List<WorkflowElement> workflowElements, Dictionary<YamlStep, int> workflowToStep, int workflowNum)
        {
            List<YamlStep> steps = new List<YamlStep>();
            foreach (WorkflowElement element in workflowElements)
            {
                //Console.WriteLine("b1");
                YamlStep step = new YamlStep();
                if (element.ElementType == WorkflowElementType.Action)
                {
                    Console.WriteLine("b2");
                    step.IsSubpipeline = (element as WorkflowAction).Subpipeline;
                    if (step.IsSubpipeline)
                    {
                        workflowNum++;
                        workflowToStep.Add(step, workflowNum);
                    }

                    Console.WriteLine("b3");
                    FillStepDetails(step, (element as WorkflowAction));
                    Console.WriteLine(step.Condition);
                    if ((element as WorkflowAction).Dependencies != null && (element as WorkflowAction).Dependencies.Count > 0)
                    {
                        step.Dependencies = new HashSet<string>((element as WorkflowAction).Dependencies);
                        // Need to add all dependencies
                        step.Previous = (element as WorkflowAction).Dependencies.First();
                    }
                    Console.WriteLine("b5");
                    steps.Add(step);
                }
                Console.WriteLine("b52");
                if (element.ElementType == WorkflowElementType.Control)
                {
                    Console.WriteLine("b51");
                    if ((element as WorkflowControl).ControlType == WorkflowControlType.Switch)
                    {
                        Console.WriteLine("b6");
                        step.Name = "condition";
                        step.ID = (element as WorkflowSwitchControl).ID;
                        step.IsConditional = true;
                        Console.WriteLine("b7");
                        foreach (var pair in (element as WorkflowSwitchControl).SwitchCases)
                        {
                            ArgoYamlFlow subPipeline = new ArgoYamlFlow();
                            subPipeline.Steps = GenerateConditionPipeline(pair.Value, workflowToStep, workflowNum).ToArray();
                            step.conditionPipelines.Add(pair.Key, subPipeline);
                        }
                        Console.WriteLine("b8");
                        if ((element as WorkflowSwitchControl).Dependencies != null && (element as WorkflowSwitchControl).Dependencies.Count > 0)
                        {
                            step.Dependencies = new HashSet<string>((element as WorkflowSwitchControl).Dependencies);
                            // Need to add all dependencies
                            step.Previous = (element as WorkflowSwitchControl).Dependencies.First();
                        }
                        Console.WriteLine("b9");

                        YamlStep actionForConditional = new YamlStep();
                        WorkflowAction action = (element as WorkflowSwitchControl).WorkflowControlAction;
                        FillStepDetails(actionForConditional, action);
                        step.ActionForConditional = actionForConditional;

                        steps.Add(step);
                    }
                }
                
            }
            return steps;
        }

        private void FillStepDetails(YamlStep step, WorkflowAction workflowAction)
        {
            step.ID = workflowAction.ID;
            step.Name = workflowAction.Title.Replace(' ', '-');
            step.Condition = workflowAction.Condition;
            step.Implementation = workflowAction.Parameters?.Implementation;
            step.Image = workflowAction.Parameters?.Image;
            step.Additional = workflowAction.Parameters?.Additional;
            step.ResourceProvider = workflowAction.Parameters?.ResourceProvider;
            step.EnvParams = workflowAction.Parameters?.EnvironmentParameters.ToDictionary((ep) => ep.Key, (ep) => ep.Value);
        }
    }
}
