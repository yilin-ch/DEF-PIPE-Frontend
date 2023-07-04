using System;
using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataCloud.PipelineDesigner.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataCloud.PipelineDesigner.Services.Transformers
{
	public class SimpleYamlTransformer: IYAMLTransformer
    {
        public Workflow Workflow { get; set; }
        public ArgoYamlFlow yaml { get; set; }

        private int taskNum;
        private int dagNum;
        private Dictionary<string, YamlStep> IDtoStep;
        private HashSet<string> templateNames;
        private StringBuilder stepTemplates;

        public SimpleYamlTransformer()
        {
        }

        public string Transform(ArgoYamlFlow yaml)
        {
            StringBuilder yamlBuilder = new StringBuilder();

            GenerateHeader(yamlBuilder, yaml);
            GenerateSpec(yamlBuilder, yaml);

            return yamlBuilder.ToString();
        }

        private void GenerateHeader(StringBuilder yamlBuilder, ArgoYamlFlow yaml)
        {
            yamlBuilder.AppendLine("apiVersion: argoproj.io/v1alpha1");
            yamlBuilder.AppendLine("kind: Workflow");
            yamlBuilder.AppendLine("metadata:");
            yamlBuilder.AppendLine(Identation(0) + "name: " + yaml.Name);
            Console.WriteLine("k1");
        }

        private void GenerateSpec(StringBuilder yamlBuilder, ArgoYamlFlow yaml, int level = 0)
        {
            taskNum = 0;
            dagNum = 0;
            IDtoStep = new Dictionary<string, YamlStep>();
            templateNames = new HashSet<string>();

            stepTemplates = new StringBuilder();

            yamlBuilder.AppendLine("spec:");
            yamlBuilder.AppendLine(Identation(level) + "entrypoint: main-workflow");
            yamlBuilder.AppendLine(Identation(level) + "templates:");
            GenerateName(yamlBuilder, level + 1, "main-workflow");

            // generate dag
            Console.WriteLine("k2");
            yamlBuilder.Append(GenerateDAG(yaml, level + 2));
            
            yamlBuilder.AppendLine();
            yamlBuilder.Append(stepTemplates);
        }

        private StringBuilder GenerateDAG(ArgoYamlFlow yaml, int level = 0)
        {
            StringBuilder dagBuilder = new StringBuilder();
            dagBuilder.AppendLine(Identation(level) + "dag:");
            dagBuilder.AppendLine(Identation(level + 1) + "tasks:");
            // allocate each step name a task name and a template name
            foreach (var step in yaml.Steps)
            {
                Console.WriteLine("k3");
                IDtoStep[step.ID] = step;
                taskNum++;
                step.TaskName = "task-" + taskNum;

                Console.WriteLine("k4");
                int nameNum = 1;
                string nameOption = step.Name.ToLower();
                if (templateNames.Contains(step.Name.ToLower()))
                    while (templateNames.Contains(nameOption))
                    {
                        nameOption = step.Name.ToLower() + nameNum;
                        nameNum++;
                    }
                step.TemplateName = nameOption;
                templateNames.Add(step.TemplateName);
                Console.WriteLine("k5");
            }
            // create tasks in dag
            foreach (var step in yaml.Steps)
            {
                Console.WriteLine("l");
                GenerateStep(dagBuilder, step, level + 2, IDtoStep);
                stepTemplates.Append(GenerateTemplate(step, 1));
                stepTemplates.AppendLine();
            }

            return dagBuilder;
        }

        private void GenerateStep(StringBuilder yamlBuilder, YamlStep step, int level, Dictionary<string, YamlStep> IDtoStep)
        {
            GenerateName(yamlBuilder, level, IDtoStep[step.ID].TaskName);
            if (step.Dependencies != null && step.Dependencies.Any())
            {
                List<string> dependencies = new List<string>(step.Dependencies);
                yamlBuilder.Append(Identation(level + 1) + "dependencies: [" + IDtoStep[dependencies[0]].TaskName);
                foreach (var dependency in dependencies.Skip(1))
                    yamlBuilder.Append(", " + IDtoStep[dependency].TaskName);
                yamlBuilder.Append("]");
                yamlBuilder.AppendLine();
            }                
            // template name in argo workflow should be all lowercases
            yamlBuilder.AppendLine(Identation(level + 1) + "template: " + step.TemplateName);
        }

        private StringBuilder GenerateSteps(YamlStep step, int level)
        {
            bool first = true;
            StringBuilder stepsBuilder = new StringBuilder();
            stepsBuilder.AppendLine(Identation(level) + "steps:");
            foreach (var pair in step.conditionPipelines)
            {
                if (first)
                {
                    stepsBuilder.AppendLine(Identation(level) + "- - name: dag-" + dagNum);
                    first = false;
                }
                else
                    stepsBuilder.AppendLine(Identation(level + 1) + "- name: dag-" + dagNum);
                stepsBuilder.AppendLine(Identation(level + 2) + "template: dag-" + dagNum);
                stepsBuilder.AppendLine(Identation(level + 2) + "when: \"" + pair.Key + "\"");

                StringBuilder dagBuilder = new StringBuilder();
                GenerateName(dagBuilder, level - 1, "dag-" + dagNum);
                dagNum++;
                dagBuilder.Append(GenerateDAG(pair.Value, level));
                dagBuilder.AppendLine();
                stepTemplates.Append(dagBuilder);
            }
            return stepsBuilder;
        }

        private StringBuilder GenerateTemplate(YamlStep step, int level)
        {

            StringBuilder templateBuilder = new StringBuilder();
            GenerateName(templateBuilder, level, step.TemplateName);

            if (step.IsSubpipeline)
            {
                templateBuilder.Append(GenerateDAG(step.subPipeline, level + 1));
            }

            else if (step.IsConditional)
            {
                templateBuilder.Append(GenerateSteps(step, level + 1));
            }

            else
            {
                // adding parameters
                //if (step.EnvParams?.Count > 0 && step.ExecRequirements?.Length > 0)
                //{
                //    templateBuilder.AppendLine(Identation(level + 1) + "inputs:");
                //    templateBuilder.AppendLine(Identation(level + 2) + "parameters:");
                //    if (step.EnvParams?.Count > 0)
                //        GenerateEnvParams(templateBuilder, step.EnvParams, level + 3);
                //    if (step.ExecRequirements?.Length > 0)
                //        GenerateExecReq(templateBuilder, step.ExecRequirements, level + 3);
                //}

                //adding container
                GenerateContainer(templateBuilder, step, level + 1);
            }
            
            return templateBuilder;
        }

        private void GenerateName(StringBuilder builder, int level, string name)
        {
            builder.AppendLine(Identation(level) + "- name: " + name);
        }

        private void GenerateEnvParams(StringBuilder builder, Dictionary<string, string> envs, int level)
        {
            builder.AppendLine(Identation(level) + "env:");
            foreach (var e in envs)
            {
                builder.AppendLine(Identation(level) + "- name: " + e.Key);
                builder.AppendLine(Identation(level + 1) + "value: " + e.Value);
            }
        }

        // different in how implemented in the graphic tool
        private void GenerateExecReq(StringBuilder builder, YamlExecutionRequirements[] reqs, int level)
        {
            GenerateName(builder, level, "executionRequirements");
            builder.AppendLine(Identation(level + 1) + "value:");
            foreach (var r in reqs)
            {

            }
        }

        private void GenerateContainer(StringBuilder builder, YamlStep step, int level)
        {
            builder.AppendLine(Identation(level) + "container:");
            builder.AppendLine(Identation(level + 1) + "image: " + step.Image);
            // some commands
            builder.AppendLine(Identation(level + 1) + "command: [sh, -c]");
            builder.AppendLine(Identation(level + 1) + "args: [\"echo Env param is $env\"]");
            // env parameters
            if (step.EnvParams?.Count > 0)
                GenerateEnvParams(builder, step.EnvParams, level + 1);

        }

        private string Identation(int level)
        {
            return new String(' ', (level + 1) * 2);
        }
    }
}

