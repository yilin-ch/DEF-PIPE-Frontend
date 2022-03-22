using DataCloud.PipelineDesigner.SimpleWorkflowEngine.Actions;
using DataCloud.PipelineDesigner.SimpleWorkflowEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.SimpleWorkflowEngine
{
    class WorkflowRunner
    {
        private Workflow ParseDSL(string workflowDSL)
        {
            throw new NotImplementedException();
        }

        

        public void ExecuteWorkflow(string workflowDSL)
        {
            var workflow = ParseDSL(workflowDSL);

            List<Person> previousOutput = null;
            foreach (var step in workflow.Steps)
            {
                List<Person> stepInput = null;
                if (step.Input.Source == WorkflowStepInputSource.File)
                {
                    stepInput = File.ReadAllLines(step.Input.FilePath)
                        .Skip(1)
                        .Select(row => Person.FromCsv(row))
                        .ToList();
                }
                else
                {
                    stepInput = previousOutput;
                }


                BaseWorkflowAction<Person> stepAction = null;
                switch (step.ActionName)
                {
                    case "Sort":
                        stepAction = new SortAction<Person>(step.ActionParameters);                        
                        break;
                    case "Filter":
                        stepAction = new FilterAction<Person>(step.ActionParameters);
                        break;
                    case "Map":
                        stepAction = new MapAction<Person>(step.ActionParameters);
                        break;
                    default:
                        break;
                }

                if (stepAction != null)
                {
                    previousOutput = stepAction.Execute(stepInput, step.Output);
                }
            }
        }
    }
}
