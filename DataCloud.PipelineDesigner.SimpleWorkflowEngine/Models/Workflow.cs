using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.SimpleWorkflowEngine.Models
{
    class Workflow
    {
        public List<WorkflowStep> Steps { get; set; }
    }

    class WorkflowStep  
    {
        public WorkflowStepInput Input { get; set; }
        public WorkflowStepOutput Output { get; set; }
        public string ActionName { get; set; }
        public Dictionary<string, string> ActionParameters { get; set; }
    }

    enum WorkflowStepInputSource
    {
        File = 0,
        PreviousStep = 1
    }

    class WorkflowStepInput
    { 
        public WorkflowStepInputSource Source { get; set; }
        public string FilePath { get; set; }        
    }

    class WorkflowStepOutput
    {
        public bool WriteToFile { get; set; }
        public string FilePath { get; set; }
    }
}
