using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.WorkflowModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services
{
    public class WorkflowService : IWorkflowService
    {
       WorkflowTransformer  workflowTransformer;
        public WorkflowService()
        {
            workflowTransformer = new WorkflowTransformer();
        }

        public Workflow TransformCanvasToWorkflow(Canvas canvas)
        {
            return workflowTransformer.GenerateWorkflow(canvas);    
        }
    }
}
