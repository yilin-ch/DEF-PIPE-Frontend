using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCloud.PipelineDesigner.Services
{
    public class SimpleDSLTransfomer: IDSLTransformer
    {
        public Workflow Workflow { get; set; }
        public Dsl Dsl { get; set; }
        public SimpleDSLTransfomer()
        {            
        }


        public string Transform(Dsl dsl)
        {
            Dsl = dsl;

            StringBuilder dslBuilder = new StringBuilder();

            dslBuilder.AppendLine("Pipeline " + dsl.Name + " {");
            dslBuilder.AppendLine(Identation(0) + "steps:");
            GenerateWorkflowSteps(dslBuilder, dsl.Steps, 0);
            dslBuilder.AppendLine("}");

            return dslBuilder.ToString();
        }

        private void GenerateWorkflowSteps(StringBuilder dslBuilder, Step[] steps, int level = 0)
        {
            foreach (var step in steps)
            {
                GenerateStepName(dslBuilder, step.Name, level + 1);
                GenerateStepImplementation(dslBuilder, step.Implementation, level + 1);
                GenerateStepImage(dslBuilder, step.Image, level + 1);
                if(step.EnvParams?.Count > 0)
                    GenerateStepEnvParams(dslBuilder, step.EnvParams, level + 1);
                if (step.ExecRequirements?.Length > 0)
                    GenerateStepExcRequ(dslBuilder, step.ExecRequirements, level + 1);
                GenerateStepResrceProvider(dslBuilder, step.ResourceProvider, level + 1);
                if (step.Previous != null)
                    GenerateStepPrevious(dslBuilder, step.Previous, level + 1);

            }
        }

        private void GenerateStepName(StringBuilder dslBuilder, string name, int level = 0)
        {
            dslBuilder.AppendLine(Identation(level-1) + "-" + Identation(0) + name);
        }
        private void GenerateStepImplementation (StringBuilder dslBuilder, string implementaton, int level = 0)
        {
            dslBuilder.AppendLine(Identation(level) + "implementation: " + implementaton);
        }
        private void GenerateStepImage(StringBuilder dslBuilder, string image, int level = 0)
        {
            dslBuilder.AppendLine(Identation(level) + "image: " + image);
        }
        private void GenerateStepEnvParams(StringBuilder dslBuilder, Dictionary<string, string> envs, int level = 0)
        {
            dslBuilder.AppendLine(Identation(level) + "environmentParameters: {" );
            foreach (var e in envs)
            {
                dslBuilder.AppendLine(Identation(level + 1) + e.Key + "=" + e.Value);
            }
            dslBuilder.AppendLine(Identation(level) + "}");
        }
        private void GenerateStepExcRequ(StringBuilder dslBuilder,ExecutionRequirements[] requs, int level = 0)
        {
            dslBuilder.AppendLine(Identation(level) + "environmentParameters: :" );
            foreach (var r in requs)
            {
                dslBuilder.AppendLine(Identation(level + 1) + r.Type + " " + r.SubType + " {");
                foreach (var e in r.Requirements)
                {
                    dslBuilder.AppendLine(Identation(level + 2) + e.Key + " " + e.Value);
                }
                dslBuilder.AppendLine(Identation(level + 1) + "}");
            }
            
        }
        private void GenerateStepResrceProvider(StringBuilder dslBuilder, string rsrc, int level = 0)
        {
            dslBuilder.AppendLine(Identation(level) + "resourceProvider: " + rsrc);
        }
        private void GenerateStepPrevious(StringBuilder dslBuilder, string prev, int level = 0)
        {
            dslBuilder.AppendLine(Identation(level) + "previous: " + prev);
        }

        public Dsl Transform(string dsl)
        {
            Workflow workflow = new Workflow();
            try
            {
                var tokens = DslTokenizer.TryTokenize(dsl);
                if (!tokens.HasValue)
                {
                    throw new Exception(tokens.ErrorMessage.ToString());
                }
                else if (!DslParser.TryParse(tokens.Value, out Dsl expr, out var error, out var errorPosition))
                {
                    throw new Exception(error.ToString());
                }
                else
                {
                    return expr;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }


        private string Identation(int level)
        {
            return new String('\t', level + 1);
        }
    }
}
