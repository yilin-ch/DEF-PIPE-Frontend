using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services.Interfaces
{
    public interface IWorkflowService
    {
        List<Workflow> TransformCanvasToWorkflow(Canvas canvas);

        Dsl TransformWorkflowToDsl(List<Workflow> workflow);
    }
}
