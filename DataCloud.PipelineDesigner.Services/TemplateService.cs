using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Services.Interfaces;
using DataCloud.PipelineDesigner.Repositories;
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
        EntitiesContext db = new EntitiesContext();

        public TemplateService()
        {

        }

        public async Task<List<CanvasShapeTemplate>> GetTemplatesAsync()
        {
            Templates.Clear();
            EnsureBuiltInTemplates();

            db.BaseEntities.ToList().ForEach(entity =>
            {
                Templates.Add(CanvasShapeTemplate.ParseDBString(entity.Workflow));
            });

            return Templates;
        }

        public async Task AddOrUpdateTemplateAsync(CanvasShapeTemplate template)
        {
            Templates.Clear();
            EnsureBuiltInTemplates();

            var existingTemplate = Templates.SingleOrDefault(t => t.Id.ToLower() == template.Id.ToLower());
            if (existingTemplate != null)
            {
                Templates.Remove(existingTemplate);
            }
            String IdTemp = template.Id;
            Repositories.Entities.BaseEntity temp =  null;
            try
            {

               temp  = db.BaseEntities.First(t => t.Id == IdTemp);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
           

            if (temp == null)
            {
                Repositories.Entities.BaseEntity newEntity = new Repositories.Entities.BaseEntity
                {
                    Id = template.Id,
                    Name = template.Name,
                    Owner = "anonymous",
                    Workflow = template.ToDBString(),
                    CreatedAt = DateTime.Now,
                };

                db.BaseEntities.Add(newEntity);
            }
            else
            {
                temp.Name = template.Name;
                temp.Workflow = template.ToDBString();
                temp.ModifiedAt = DateTime.Now;
                db.BaseEntities.Update(temp);
            }



            try
            {
              
               db.SaveChanges();

            }
            catch (Exception e)
            {
              Console.WriteLine(e.ToString());
            }




            Templates.Add(template);
        }

        public async Task<bool> DeleteTemplate(String id)
        {

            Repositories.Entities.BaseEntity temp = db.BaseEntities.First(t => t.Id == id);

            try
            {

                db.Remove(temp);
                db.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }


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
