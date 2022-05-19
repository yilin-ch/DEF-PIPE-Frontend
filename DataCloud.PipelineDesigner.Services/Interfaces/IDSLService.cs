using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services.Interfaces
{
    public interface IDSLService
    {
        List<DSLInfo> GetAvailableDSL();
        public string SerializeDsl(Dsl workflow);

        public Dsl DeserializeDsl(String dsl);
    }
}
