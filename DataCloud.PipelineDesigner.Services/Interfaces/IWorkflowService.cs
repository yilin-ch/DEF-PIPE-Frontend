using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.WorkflowModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services.Interfaces
{
    public interface IWorkflowService
    {
        Workflow TransformCanvasToWorkflow(Canvas canvas);
    }
}
