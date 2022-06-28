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
            
        }

        public string SerializeDsl(Dsl dsl)
        {
            dslTransformer = new DSLTransfomer(dsl);
            return dslTransformer.Transform();
        }

        public Dsl DeserializeDsl(String dsl)
        {
            return DSLTransfomer.Parse(dsl);
        }

      
    }
}
