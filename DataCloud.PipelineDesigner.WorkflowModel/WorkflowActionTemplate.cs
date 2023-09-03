using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.WorkflowModel
{
    public class WorkflowActionTemplate
    {
        public List<WorkflowParam> Parameters { get; set; }

        public WorkflowActionTemplate()
        {
            Parameters = new List<WorkflowParam>();
        }
    }
}
