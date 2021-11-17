using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services.DSLSamples
{
    [DSLMetadata(name: "Simple DSL", description: "A sample DSL for data workflow")]
    public class SimpleDSL
    {
        [DSLTemplateProperty("Execution Class")]
        public string ExecutionClass { get; set; }

        [DSLTemplateProperty("Execution Method")]
        public string ExecutionMethod { get; set; }
    }
}
