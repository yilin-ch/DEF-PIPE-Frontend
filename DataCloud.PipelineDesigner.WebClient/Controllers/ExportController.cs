﻿using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Services;
using DataCloud.PipelineDesigner.Services.Interfaces;
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
        IDSLService dslService;
        IWorkflowService workflowService;

        public ExportController()
        {
            //TODO: Update this to use dependency injection
            dslService = new DSLService();
            workflowService = new WorkflowService();
        }

        /// <summary>
        /// DEPRECATED
        /// </summary>
        [HttpGet("dsl/available")]
        public List<DSLInfo> GetAvailableDSL()
        {
            return dslService.GetAvailableDSL();
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
                var newCanvas = new Canvas();
                foreach (var item in canvas.Elements)
                {
                    if (item.Type == CanvasElementType.Connector)
                        newCanvas.Elements.Add(JsonConvert.DeserializeObject<CanvasConnector>(JsonConvert.SerializeObject(item)));
                    else
                        newCanvas.Elements.Add(JsonConvert.DeserializeObject<CanvasShape>(JsonConvert.SerializeObject(item)));
                }

                var workflow = workflowService.TransformCanvasToWorkflow(newCanvas);
                var dsl = workflowService.TransformWorkflowToDsl(workflow, canvas.Name);

                return dslService.SerializeDsl(dsl);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
