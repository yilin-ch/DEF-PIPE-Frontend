using DataCloud.PipelineDesigner.SimpleWorkflowEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.SimpleWorkflowEngine.Actions
{
    internal class FilterAction<T> : BaseWorkflowAction<T>
    {
        public FilterAction(Dictionary<string, string> parameters): base(parameters)
        {

        }

        public override List<T> Execute(List<T> input, WorkflowStepOutput outputSettings)
        {
            var sortProperty = Parameters["sortProperty"];
            var sortOrder = Parameters["sortOrder"];

            var output = new List<T>();
            output.AddRange(input);

            output.Sort((a, b) =>
            {
                var value1 = ReflectionHelper.GetPropertyValue(a, sortProperty);
                var value2 = ReflectionHelper.GetPropertyValue(b, sortProperty);

                if (value1 == value2)
                    return 0;
                else
                    return value1.CompareTo(value2);
            });

            if (outputSettings.WriteToFile)
            {
                //TODO: Write to file
            }

            return output;
        }
    }
}
