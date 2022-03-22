using DataCloud.PipelineDesigner.SimpleWorkflowEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.SimpleWorkflowEngine.Actions
{
    internal class MapAction<T> : BaseWorkflowAction<T>
    {
        public MapAction(Dictionary<string, string> parameters) : base(parameters)
        {

        }
        public override List<T> Execute(List<T> input, WorkflowStepOutput outputSettings)
        {
            throw new NotImplementedException();
        }
    }
}
