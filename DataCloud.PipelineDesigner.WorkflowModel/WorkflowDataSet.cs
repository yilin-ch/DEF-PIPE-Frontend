using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.WorkflowModel
{
    public class WorkflowDataSet
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public WorkflowDataSetFormat Format { get; set; }

        public WorkflowDataSet()
        {
            Parameters = new Dictionary<string, string>();
        }
    }
}
