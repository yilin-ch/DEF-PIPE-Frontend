using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.WorkflowModel
{
    public class WorkflowParam
    {
        public string Name { get; set; }
        public WorkflowParamType Type { get; set; }
        public bool Required { get; set; }
    }
}
