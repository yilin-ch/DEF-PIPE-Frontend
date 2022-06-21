using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services
{
    public class DSLService: IDSLService
    {
        IDSLTransformer dslTransformer;

        public DSLService()
        {
            //TODO: Update this to use dependency injection
            dslTransformer = new SimpleDSLTransfomer();
        }

        public string SerializeDsl(Dsl workflow)
        {
            return dslTransformer.Transform(workflow);
        }

        public Dsl DeserializeDsl(String dsl)
        {
            return dslTransformer.Transform(dsl);
        }

      
    }
}
