using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.CanvasModel
{

    
    public class CanvasShape : CanvasElement
    {
        //public new CanvasElementType Type => CanvasElementType.Shape;
        public string Name { get; set; }
        public CanvasPosition Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Conditional { get; set; }
        public string Condition { get; set; }
        public string Loop { get; set; }
        public string LoopCondition { get; set; }
        public List<CanvasElement> Elements { get; set; }
        public CanvasShapeTemplate Template { get; set; }
        public string TemplateId { get; set; }
        public CanvasParameters Parameters;
        [JsonProperty("Properties")]
        public List<CanvasElementProperty> Properties { get; set; }

        private Dictionary<string, string> properties;
        public Dictionary<string, string> PropertiesDict {
            get
            {
                if (properties == null)
                {
                    properties = new Dictionary<string, string>();
                    Properties?.ForEach(p => properties.Add(p.Name, p.Value));
                }
                return properties;
            }
        }

        public List<CanvasShapeConnectionPoint> ConnectionPoints { get; set; }
        public string Shape { get; set; }

        public CanvasShape()
        {
            Elements = new List<CanvasElement>();
            Position = new CanvasPosition();
        }
    }

    
}
