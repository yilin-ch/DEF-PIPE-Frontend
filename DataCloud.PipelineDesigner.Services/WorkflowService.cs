using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.WorkflowModel;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using Newtonsoft.Json;
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

        public List<Workflow> TransformCanvasToWorkflow(Canvas canvas)
        {
            return workflowTransformer.GenerateWorkflows(new List<Workflow>(), new List<Canvas> { canvas});    
        }
        public Dsl TransformWorkflowToDsl(List<Workflow> workflow, List<CanvasProvider> providers)
        {
            return workflowTransformer.GenerateDsl(workflow, providers);    
        }
    }
}
