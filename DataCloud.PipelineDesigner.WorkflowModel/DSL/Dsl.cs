using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.WorkflowModel.DSL
{
    public class Dsl
    {
        public Pipeline Pipeline { get; set; }
        public Pipeline[] SubPipelines { get; set; }
        public ResourceProvider[] ResourceProvider{ get; set; }

        public Dsl()
        {
            Pipeline = new Pipeline();
        }

        public override string ToString()
        {
            return this.Pipeline.ToString("Pipeline") +
                 (this.SubPipelines.Length > 0 ? string.Join("", this.SubPipelines?.Select(s => s.ToString("Subpipline"))) : "") +
                 (this.ResourceProvider != null ?  string.Join("\n\n", this.ResourceProvider.Select(r => r)) : "");
        }


    }
    public class Pipeline
    {
        public string Name { get; set; }
        public Step[] Steps { get; set; }
        public CommunicationMedium CommunicationMedium { get; set; }
        public Dictionary<string, string> EnvParams { get; set; }

        public string ToString(string pipeline)
        {
            return pipeline+" " + this.Name + " {\n" +
                "\tcommunicationMedium: medium " + this.CommunicationMedium?.Type + 
                (this.EnvParams != null ?
                "\n\tenvironmentParameters: {\n" +
                string.Join(",\n", this.EnvParams?.Select(kv => "\t\t" + kv.Key + ": '" + kv.Value + "'").ToArray()) +
                "\n\t}" : "") +
                "\n\tsteps:\n" +
                string.Join("\n\n", this.Steps.Select(s => s)) +
                "\n}\n\n";
        }
    }

    public class CommunicationMedium
    {
        public string Type { get; set; }

    }


    public class Step
    {
        public string Name { get; set; }
        public string StepType { get; set; }
        public string Type { get; set; }
        public Implementation Implementation { get; set; }

        public Dictionary<string, string> EnvParams { get; set; }

        public ExecutionRequirements[] ExecRequirements { get; set; }

        public string ResourceProvider { get; set; }
        public string[] Previous { get; set; }

        public override string ToString()
        {
            return "\t\t- " + this.StepType +" "+ this.Type + " " + this.Name +
                "\n\t\t\t" + this.Implementation.ToString() +
                 (this.EnvParams != null ?
                "\n\t\t\tenvironmentParameters: {\n" +
                string.Join(",\n", this.EnvParams?.Select(kv => "\t\t\t\t" + kv.Key + ": '" + kv.Value + "'").ToArray()) +
                "\n\t\t\t}" : "") +
                (this.ResourceProvider != null ? "\n\t\t\tresourceProvider: " + this.ResourceProvider.ToString() : "") +
                (this.ExecRequirements != null ? "\n\t\t\texecutionRequirement: " + string.Join("\n", this.ExecRequirements?.Select(e => e)): "") ;
                //+ "\n\t\t\tprevious: [" + string.Join(", ", this.Previous?.Select(e => e)) + "]";
        }
    }

    public class Implementation
    {
        public string Type { get; set; }

        public String ImageName { get; set; }

        public override string ToString()
        {
            return "implementation: " + this.Type + " image:  '" + this.ImageName + "'";
        }
    }

    public class ExecutionRequirements
    {
        public string Type { get; set; }

        public RequirementsSubType[] SubTypeRequirements { get; set; }

        public override string ToString()
        {
            return "\n\t\t\t\t" + this.Type + ":" +
                "\t\t\t\t\t" + string.Join("\n", this.SubTypeRequirements?.Select(e => e));
        }
    }

    public class RequirementsSubType
    {
        public string SubType { get; set; }

        public Dictionary<string, string> Requirements { get; set; }

        public override string ToString()
        {
            return "\n\t\t\t\t\t" + this.SubType + ":\n" +
                string.Join("\n", this.Requirements.Select(kv => "\t\t\t\t\t\t" + kv.Key + ": " + kv.Value).ToArray());
        }
    }

    public class ResourceProvider
    {
        public String Provider { get; set; }
        public String Name { get; set; }
        public String ProviderLocation { get; set; }
        public String MappingLocation { get; set; }

        public override string ToString()
        {
            return this.Provider + " " + this.Name + " {" +
                "\n\tproviderLocation: '" + this.ProviderLocation + "'" +
                "\n\tmappingLocation: '" + this.MappingLocation + "'" +
                "\n}";
        }

    }


}
