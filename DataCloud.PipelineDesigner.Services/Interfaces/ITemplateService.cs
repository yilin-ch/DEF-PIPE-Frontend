using DataCloud.PipelineDesigner.CanvasModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services.Interfaces
{
    public interface ITemplateService
    {
        Task AddOrUpdateTemplateAsync(CanvasShapeTemplate template);
        Task<List<CanvasShapeTemplate>> GetTemplatesAsync();
        Task<bool> DeleteTemplate(String id);
    }
}
