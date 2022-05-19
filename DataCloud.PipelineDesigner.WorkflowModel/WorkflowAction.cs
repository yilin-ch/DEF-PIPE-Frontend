using DataCloud.PipelineDesigner.CanvasModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCloud.PipelineDesigner.WorkflowModel
{
    public class WorkflowAction: WorkflowElement
    {
        public override WorkflowElementType ElementType => WorkflowElementType.Action;
        public string ID { get; set; }
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

        public WorkflowAction(CanvasParameters canvasParameters)
        {
            if (canvasParameters != null)
            {
                Parameters = new WorkflowParameters
                {
                    Implementation = canvasParameters.Implementation,
                    Image = canvasParameters.Image,
                    EnvironmentParameters = canvasParameters.EnvironmentParameters.Select(ep => new EnvironmentParameter { Key = ep.Key, Value = ep.Value }).ToList(),
                    ResourceProvider = canvasParameters.ResourceProvider,
                };
            }

            //Template = template ?? throw new Exception("WorkflowActionTemplate cannot be null");
           // Parameters = new Dictionary<string, string>();
        }
    }

    public class WorkflowParameters
    {
        public string Implementation { get; set; }
        public string Image { get; set; }
        public List<EnvironmentParameter> EnvironmentParameters { get; set; }
        public string ResourceProvider { get; set; }
    }

    public class EnvironmentParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
