using DataCloud.PipelineDesigner.CanvasModel;
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
        public Dsl TransformWorkflowToDsl(Workflow workflow, string name)
        {
            return workflowTransformer.GenerateDsl(workflow, name);    
        }
    }
}
