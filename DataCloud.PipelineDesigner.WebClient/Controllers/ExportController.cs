using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Services;
using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.Services.Transformers;
using DataCloud.PipelineDesigner.WorkflowModel.DSL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.WebClient.Controllers
{
    [Route("api/export")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        IYAMLService yamlService;
        IDSLService dslService;
        IWorkflowService workflowService;

        public ExportController()
        {
            //TODO: Update this to use dependency injection
            yamlService = new YAMLService();
            dslService = new DSLService();
            workflowService = new WorkflowService();
        }

        /// <summary>
        /// Transform workflow to dsl
        /// </summary>
        /// <response code="200">Workflow in DSL format</response>
        [HttpPost("dsl")]
        public string ExportToDSL([FromBody] Canvas canvas)
        {
            try
            {

                var workflow = workflowService.TransformCanvasToWorkflow(canvas);
                var dsl = workflowService.TransformWorkflowToDsl(workflow, canvas.ResourceProviders);

                return dslService.SerializeDsl(dsl);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [HttpPost("yaml")]
        public string ExportToYAML([FromBody] Canvas canvas)
        {
            try
            {

                var workflow = workflowService.TransformCanvasToWorkflow(canvas);
                var yaml = workflowService.TransformWorkflowToYaml(workflow, canvas.Name);

                Console.WriteLine("k");
                return yamlService.SerializeYaml(yaml);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
