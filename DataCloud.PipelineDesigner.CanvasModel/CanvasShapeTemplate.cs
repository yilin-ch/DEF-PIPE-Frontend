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
