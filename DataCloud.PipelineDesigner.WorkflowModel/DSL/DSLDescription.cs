using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.WorkflowModel.DSL
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DSLMetadata: Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DSLMetadata(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DSLTemplateProperty : Attribute
    {
        public string Name { get; set; }
        public DSLTemplateProperty(string name)
        {
            Name = name;
        }
    }
}
