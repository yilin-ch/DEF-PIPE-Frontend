using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCloud.PipelineDesigner.Services
{
    class CanvasService
    {
/*
        public static CanvasShapeTemplate ToCanvasShapeTemplate(Repositories.Entities.Template t)
        {

            List<CanvasElement> elements = new List<CanvasElement>();
            foreach (Template e in t.Elements)
            {
                elements.Add(new CanvasShape
                {
                ID = e.Id,
                Name = e.Name,
                Shape = (t.Elements?.Count > 0) ? "Container" : "Rectangle",
                Properties = e.Properties.Select(p => new CanvasElementProperty
                {
                    Name = p.Name,
                    Value = p.Value,
                }).ToList(),
                ConnectionPoints = e.Connections.Select(c => new CanvasShapeConnectionPoint
                    {
                        Id = c.Id,
                        Position = new CanvasPosition { X= c.X, Y= c.Y },
                        Type = c.Output ? CanvasConnectionPointType.Output : CanvasConnectionPointType.Input,
                }).ToList(),
                });
            }



            return new CanvasShapeTemplate
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Category = t.Category?.Name,
                Width = t.Width,
                Height = t.Height,
                Elements = elements,
                Shape = (t.Elements?.Count > 0) ? "Container" : "Rectangle",
                ConnectionPoints = t.Connections.Select(c => new CanvasShapeConnectionPoint
                {
                    Id = c.Id,
                    Position = new CanvasPosition { X = c.X, Y = c.Y },
                    Type = c.Output ? CanvasConnectionPointType.Output : CanvasConnectionPointType.Input,

                }).ToList(),
                Properties = t.Properties.Select(c => new CanvasElementProperty
                {
                    Id = c.Id,
                    Name= c.Name,
                    Value = c.Value,
                    Type  = CanvasElementPropertyType.MultiLineText,
                    AllowEditing =c.Editable


                }).ToList(),
                IsContainer = (t.Elements?.Count > 0)




            };
        }


        public static Template ToTemplateEntity(CanvasShape cs)
        {
            Template t = new Template
            {
                //Id = cs.ID,
                Name = cs.Name,
                Elements = cs.Elements.OfType<CanvasShape>().Select(e => ToTemplateEntity(e)).ToList(),

                Connections = cs.ConnectionPoints.Select(c => new Connection
                {
                    //Id = c.Id,
                    X = c.Position.X,
                    Y = c.Position.Y,
                    Output = c.Type == CanvasConnectionPointType.Output,
                    TemplateId = cs.ID,

                }).ToList(),
                Properties = cs.Properties.Select(c => new Parameter
                {
                    //Id = c.Id,
                    Name = c.Name,
                    Value = c.Value,
                    Editable = (bool)c.AllowEditing,

                }).ToList(),



            };

            return t;
        }

        public static Template ToTemplateEntity(CanvasShapeTemplate t)
        {

           

            return new Template
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                CategoryId = t.Category,
                Width = t.Width,
                Height = t.Height,
                Elements = t.Elements.OfType<CanvasShape>().Select(e => ToTemplateEntity(e)).ToList(),
                
                Connections = t.ConnectionPoints.Select(c => new Connection
                {
                    Id = c.Id,
                    X = c.Position.X,
                    Y = c.Position.Y,
                    Output = c.Type == CanvasConnectionPointType.Output,
                    TemplateId = t.Id,

                }).ToList(),
                Properties = t.Properties.Select(c => new Parameter
                {
                    Id = c.Id,
                    Name = c.Name,
                    Value = c.Value,
                    Editable = (bool)c.AllowEditing,

                }).ToList(),



            };
        }
*/

    }
}
