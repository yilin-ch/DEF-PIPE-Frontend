using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services.DSLSamples
{
    [DSLMetadata(name: "Yared DSL", description: "An DSL for data workflow, made by Yared DEJENE DESSALK")]
    public class YaredDSL
    {
        [DSLTemplateProperty(name: "Trigger")]
        public string Trigger { get; set; }


        [DSLTemplateProperty(name: "PathToDockerImage")]
        public string PathToDockerImage { get; set; }
    }
}
