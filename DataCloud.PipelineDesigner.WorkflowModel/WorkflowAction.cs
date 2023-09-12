using DataCloud.PipelineDesigner.CanvasModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataCloud.PipelineDesigner.WorkflowModel
{
    public class WorkflowAction: WorkflowElement
    {
        public override WorkflowElementType ElementType => WorkflowElementType.Action;
        public override string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public WorkflowActionTemplate Template { get; set; }
       // public Dictionary<string, string> Parameters { get; set; }
        public WorkflowParameters Parameters { get; set; }
        public Guid? InputDataSetId { get; set; }
        public Guid? OutputDataSetId { get; set; }
        //public string InjectedCode { get; set; }
        //public int ConcurrentInstances { get; set; }
        //public Guid HardwareConfiguration { get; set; }
        public HashSet<string> Dependencies { get; set; }
        public bool Subpipeline = false;
        public string Condition { get; set; }

        public WorkflowAction(CanvasParameters canvasParameters)
        {
            if (canvasParameters != null)
            {
                Parameters = new WorkflowParameters
                {
                    Implementation = canvasParameters.Implementation,
                    Image = canvasParameters.Image,
                    Additional = canvasParameters.Additional,
                    EnvironmentParameters = canvasParameters.EnvironmentParameters?.Select(ep => new EnvironmentParameter { Key = ep.Key, Value = ep.Value }).ToList(),
                    ResourceProvider = canvasParameters.ResourceProvider,
                    StepType = canvasParameters.StepType,
                    StepImplementation = canvasParameters.StepImplementation,
                    ExecutionRequirement = canvasParameters.ExecutionRequirement,
                };
            }

            //Template = template ?? throw new Exception("WorkflowActionTemplate cannot be null");
           // Parameters = new Dictionary<string, string>();
        }

        public WorkflowAction(CanvasParameters canvasParameters, string canvasID)
        {
            if (canvasParameters != null)
            {
                Parameters = new WorkflowParameters
                {
                    Implementation = canvasParameters.Implementation,
                    Image = canvasParameters.Image,
                    Additional = canvasParameters.Additional,
                    EnvironmentParameters = canvasParameters.EnvironmentParameters.Select(ep => new EnvironmentParameter { Key = ep.Key, Value = ep.Value }).ToList(),
                    ResourceProvider = canvasParameters.ResourceProvider,
                    StepType = canvasParameters.StepType,
                    StepImplementation = canvasParameters.StepImplementation,
                    ExecutionRequirement = canvasParameters.ExecutionRequirement,
                };
            }

            this.ID = canvasID;
            this.Dependencies = new HashSet<string>();

            //Template = template ?? throw new Exception("WorkflowActionTemplate cannot be null");
            // Parameters = new Dictionary<string, string>();
        }
    }

    public class WorkflowParameters
    {
        public string Implementation { get; set; }
        public string Image { get; set; }
        public string Additional { get; set; }
        public List<EnvironmentParameter> EnvironmentParameters { get; set; }
        public string ResourceProvider { get; set; }
        public string StepType { get; set; }
        public string StepImplementation { get; set; }

        public dynamic ExecutionRequirement { get; set; }
    }

    public class EnvironmentParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
