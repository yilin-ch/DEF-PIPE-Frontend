using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.WorkflowModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCloud.PipelineDesigner.Services
{
    public class SimpleDSLTransfomer: IDSLTransformer
    {
        public Workflow Workflow { get; set; }
        public SimpleDSLTransfomer()
        {            
        }

        public string Transform(Workflow workflow, string name)
        {
            Workflow = workflow;

            StringBuilder dslBuilder = new StringBuilder();

            dslBuilder.AppendLine("Pipeline " + name + " {");

            //GenerateWorkflowProperties(dslBuilder);
            GenerateWorkflowSteps(dslBuilder, workflow.Elements);
            dslBuilder.AppendLine("}");

            return dslBuilder.ToString();
        }

        public Workflow Transform(string dsl)
        {
            Workflow workflow = new Workflow();
            try
            {
                var tokens = DslTokenizer.TryTokenize(dsl);
                if (!tokens.HasValue)
                {
                    throw new Exception(tokens.ErrorMessage.ToString());
                }
                else if (!DslParser.TryParse(tokens.Value, out var expr, out var error, out var errorPosition))
                {
                    throw new Exception(error.ToString());
                }
                else
                {
                    var e = expr;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            

            return workflow;
        }

        private void GenerateWorkflowProperties(StringBuilder dslBuilder, int level = 0)
        {
            dslBuilder.AppendLine(Identation(level) + "input:");
            dslBuilder.AppendLine(Identation(level + 1) + Workflow.Parameters["Input path"]);

            var remainingParams = Workflow.Parameters.Where(p => p.Key != "Input path").ToList();
            if (remainingParams.Count > 0)
            {
                dslBuilder.AppendLine(Identation(level) + "parameters:");
                foreach (var param in remainingParams)
                {
                    dslBuilder.AppendLine(Identation(level + 1) + param.Key + ": " + param.Value);
                }
            }
        }

        private void GenerateWorkflowSteps(StringBuilder dslBuilder, List<WorkflowElement> elements, int level = 0)
        {
            dslBuilder.AppendLine(Identation(level) + "steps:");
            foreach (var element in elements)
            {
                switch (element.ElementType)
                {
                    case WorkflowElementType.Action:
                        GenerateWorkflowStep(dslBuilder, element as WorkflowAction, level + 1);
                        break;
                    case WorkflowElementType.Control:
                        if (element is WorkflowSwitchControl)
                        {
                            GenerateWorkflowStep(dslBuilder, element as WorkflowSwitchControl, level + 1);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void GenerateWorkflowStep(StringBuilder dslBuilder, WorkflowSwitchControl workflowControl, int level)
        {
            dslBuilder.AppendLine(Identation(level) + "- step Branching");

            if (workflowControl.InputDataSetId != null)
            {
                dslBuilder.AppendLine(Identation(level + 1) + "input:");
                var inputDataSet = Workflow.DataSets.FirstOrDefault(p => p.Id == workflowControl.InputDataSetId);
                dslBuilder.AppendLine(Identation(level + 2) + inputDataSet.Parameters["File Path"]);
            }

            dslBuilder.AppendLine(Identation(level + 1) + "switch:");
            foreach (var branch in workflowControl.SwitchCases)
            {
                dslBuilder.AppendLine(Identation(level + 2) + "case: " + branch.Key);
                GenerateWorkflowSteps(dslBuilder, branch.Value, level + 3);
            }

            dslBuilder.AppendLine(Identation(level + 2) + "default: ");
            GenerateWorkflowSteps(dslBuilder, workflowControl.Elements, level + 3);

            dslBuilder.AppendLine();
        }

        private void GenerateWorkflowStep(StringBuilder dslBuilder, WorkflowAction workflowAction, int level)
        {
            dslBuilder.AppendLine(Identation(level) + "data-source step: " + workflowAction.Title);

            if (workflowAction.InputDataSetId != null)
            {
                dslBuilder.AppendLine(Identation(level + 1) + "input:");
                var inputDataSet = Workflow.DataSets.FirstOrDefault(p => p.Id == workflowAction.InputDataSetId);
                dslBuilder.AppendLine(Identation(level + 2) + inputDataSet.Parameters["File Path"]);
            }

            if (workflowAction.OutputDataSetId != null)
            {
                dslBuilder.AppendLine(Identation(level + 1) + "output:");
                var outputDataSet = Workflow.DataSets.FirstOrDefault(p => p.Id == workflowAction.OutputDataSetId);
                dslBuilder.AppendLine(Identation(level + 2) + outputDataSet.Parameters["File Path"]);
            }

            if (workflowAction.Parameters != null)
            {
                dslBuilder.AppendLine(Identation(level) + "implementation: " + workflowAction.Parameters.Implementation);

                dslBuilder.AppendLine(Identation(level) + "environmentParameters: {");
                foreach (var envParam in workflowAction.Parameters.EnvironmentParameters)
                {
                    dslBuilder.AppendLine(Identation(level + 1) + envParam.Key + "=" + envParam.Value);
                }
                dslBuilder.AppendLine(Identation(level) + "}");
                dslBuilder.AppendLine(Identation(level) + "resourceProvider: " + workflowAction.Parameters.ResourceProvider);
            }
            


            /*
            dslBuilder.AppendLine(Identation(level + 1) + "paramters:");
            foreach (var actionParam in workflowAction.Parameters)
            {
                dslBuilder.AppendLine(Identation(level + 2) + actionParam.Key + ": " + actionParam.Value);
            }
            */

            dslBuilder.AppendLine();
        }


        private string Identation(int level)
        {
            return new String('\t', level + 1);
        }

    }
}
