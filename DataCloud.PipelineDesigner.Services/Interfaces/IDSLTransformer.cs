using DataCloud.PipelineDesigner.WorkflowModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services.Interfaces
{
    public interface IDSLTransformer
    {       
        string Transform(Workflow workflow);
        Workflow Transform(String dsl);
    }
}
