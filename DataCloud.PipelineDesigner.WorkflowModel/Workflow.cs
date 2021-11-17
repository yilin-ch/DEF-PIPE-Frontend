using System;
using System.Collections.Generic;

namespace DataCloud.PipelineDesigner.WorkflowModel
{
    public class Workflow
    {
        public List<WorkflowElement> Elements { get; set; }
        public List<WorkflowDataSet> DataSets { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        
        public Workflow()
        {
            Elements = new List<WorkflowElement>();
            DataSets = new List<WorkflowDataSet>();
            Parameters = new Dictionary<string, string>();
        }
    }
}
