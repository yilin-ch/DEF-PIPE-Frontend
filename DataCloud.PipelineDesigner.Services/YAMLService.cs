using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.Services.Transformers;
using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services
{
    public class YAMLService : IYAMLService
    {
        IYAMLTransformer yamlTransformer;

        public string SerializeYaml(ArgoYamlFlow yaml)
        {
            yamlTransformer = new SimpleYamlTransformer();
            return yamlTransformer.Transform(yaml);
        }


    }
}
