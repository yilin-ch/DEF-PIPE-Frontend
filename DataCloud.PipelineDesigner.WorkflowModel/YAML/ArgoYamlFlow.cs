using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.WorkflowModel
{
    public class ArgoYamlFlow
    {
        public string Name { get; set; }
        public bool IsLoop = false;
        public string LoopCondition;
        public YamlStep[] Steps { get; set; }

    }

    public class YamlStep
    {
        public bool IsSubpipeline;
        public ArgoYamlFlow subPipeline;

        public bool IsConditional;
        public YamlStep ActionForConditional;
        public Dictionary<string, ArgoYamlFlow> conditionPipelines { get; set; }

        public string Name { get; set; }

        public string TaskName { get; set; }
        public string TemplateName { get; set; }

        public string Condition { get; set; }

        public string Implementation { get; set; }

        public string ID { get; set; }

        public string Image { get; set; }

        public string Additional { get; set; }

        public Dictionary<string, string> EnvParams { get; set; }

        public YamlExecutionRequirements[] ExecRequirements { get; set; }

        public string ResourceProvider { get; set; }
        public string Previous { get; set; }
        public HashSet<string> Dependencies { get; set; }
    }

    public class YamlExecutionRequirements
    {
        public string Type { get; set; }
        public string SubType { get; set; }

        public Dictionary<string, string> Requirements { get; set; }
    }
}
