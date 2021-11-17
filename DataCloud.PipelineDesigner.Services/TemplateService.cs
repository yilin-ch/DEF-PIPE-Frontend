using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services
{
    public class TemplateService: ITemplateService
    {
        // Use In-memory storage for testing until DB layer is added.
        static List<CanvasShapeTemplate> Templates = new List<CanvasShapeTemplate>();

        public TemplateService()
        {

        }

        public async Task<List<CanvasShapeTemplate>> GetTemplatesAsync()
        {
            EnsureBuiltInTemplates();

            return Templates;
        }

        public async Task AddOrUpdateTemplateAsync(CanvasShapeTemplate template)
        {
            EnsureBuiltInTemplates();

            var existingTemplate = Templates.SingleOrDefault(t => t.Id.ToLower() == template.Id.ToLower());
            if (existingTemplate != null)
            {
                Templates.Remove(existingTemplate);
            }

            Templates.Add(template);
        }

        private void EnsureBuiltInTemplates()
        {
            if (Templates.Count == 0)
            {
                Templates.AddRange(Constants.BuiltInTemplates);
                Templates.AddRange(Constants.SimpleDSLTemlates);
            }
        }
    }
}
