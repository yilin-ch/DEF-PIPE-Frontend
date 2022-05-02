using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Repositories.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services.Interfaces
{
    public interface ITemplateService
    {
        Task AddOrUpdateTemplateAsync(Template template);
        Task<List<Template>> GetTemplatesAsync();
        Task<DeleteResult> DeleteTemplate(String id);
    }
}
