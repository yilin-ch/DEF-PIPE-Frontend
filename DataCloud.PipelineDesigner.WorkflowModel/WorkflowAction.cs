using System;
using System.Collections.Generic;
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
        public Dictionary<string, string> Parameters { get; set; }
        public Guid? InputDataSetId { get; set; }
        public Guid? OutputDataSetId { get; set; }
        //public string InjectedCode { get; set; }
        //public int ConcurrentInstances { get; set; }
        //public Guid HardwareConfiguration { get; set; }

        public WorkflowAction()
        {
            //Template = template ?? throw new Exception("WorkflowActionTemplate cannot be null");
            Parameters = new Dictionary<string, string>();
        }
    }
}
