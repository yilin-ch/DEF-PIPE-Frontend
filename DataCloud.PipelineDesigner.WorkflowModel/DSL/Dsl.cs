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

    }

    public class Pipeline
    {
        public string Name { get; set; }
        public Step[] Steps { get; set; }
        public CommunicationMedium CommunicationMedium { get; set;}

    }

    public class CommunicationMedium
    {
        public string Type { get; set; }

    }


    public class Step
    {
        public string Name { get; set; }
        public Implementation Implementation { get; set; }

        public  Dictionary<string, string> EnvParams { get; set; }

        public ExecutionRequirements[] ExecRequirements { get; set; }

        public string ResourceProvider { get; set; }
        public string Previous { get; set; }
    }

    public class Implementation
    {
        public string Type { get; set; }

        public String ImageName { get; set; }
    }

    public class ExecutionRequirements
    {
        public string Type { get; set; }

        public RequirementsSubType[] SubTypeRequirements { get; set; }
    }

    public class RequirementsSubType
    {
        public string SubType { get; set; }

        public Dictionary<string, string> Requirements { get; set; }
    }

    public class ResourceProvider
    {
        public String Provider { get; set; }
        public String name { get; set; }
        public String ProviderLocation { get; set; }
        public String MappingLocation { get; set; }

    }


}
