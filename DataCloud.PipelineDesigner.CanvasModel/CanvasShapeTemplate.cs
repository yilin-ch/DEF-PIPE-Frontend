using System;
using System.Collections.Generic;
using System.Text;

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
        public string Shape { get; set; }
        public string ShapeImageBase64 { get; set; }
        public List<CanvasShapeConnectionPoint> ConnectionPoints { get; set; }
        public List<CanvasElementProperty> Properties { get; set; }
        public bool? IsContainer { get; set; }

        public CanvasShapeTemplate()
        {
            ConnectionPoints = new List<CanvasShapeConnectionPoint>();
        }
    }
}
