using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace DataCloud.PipelineDesigner.CanvasModel
{
    public class CanvasShapeTemplate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<CanvasElement> Elements { get; set; }
        public string Shape { get; set; }
        public string ShapeImageBase64 { get; set; }
        public List<CanvasShapeConnectionPoint> ConnectionPoints { get; set; }
        public List<CanvasElementProperty> Properties { get; set; }
        public bool? IsContainer { get; set; }


        public CanvasShapeTemplate()
        {
            ConnectionPoints = new List<CanvasShapeConnectionPoint>();
        }

        public CanvasShapeTemplate(string name, string description, string category)
        {
            this.Width = 200;
            this.Height = 100;
            this.Shape = "Container";
            this.IsContainer = true;
            this.ConnectionPoints = new List<CanvasShapeConnectionPoint>() {
                    new CanvasShapeConnectionPoint { Id = "1", Position = new CanvasPosition { X = 0, Y = 50 }, Type= CanvasConnectionPointType.Input },
                    new CanvasShapeConnectionPoint { Id = "2", Position = new CanvasPosition { X = 200, Y = 50 }, Type = CanvasConnectionPointType.Output }
                };
            this.Elements = new List<CanvasElement>();
            Properties = new List<CanvasElementProperty>();
        }

        public string ToDBString()
        {
            return JsonConvert.SerializeObject(this);
        }
        static public CanvasShapeTemplate ParseDBString(String jsonString)
        {
            CanvasShapeTemplate? canvasShapeTemplate =
              JsonConvert.DeserializeObject<CanvasShapeTemplate>(jsonString);

            return canvasShapeTemplate;
        }
    }
}
