using DataCloud.PipelineDesigner.SimpleWorkflowEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.SimpleWorkflowEngine.Actions
{
    internal abstract class BaseWorkflowAction<T>
    {        
        public Dictionary<string, string> Parameters { get; set; }
        public abstract List<T> Execute(List<T> input, WorkflowStepOutput outputSettings);

        public BaseWorkflowAction(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
        }
    }
}
