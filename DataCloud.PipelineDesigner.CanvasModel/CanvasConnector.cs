using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.CanvasModel
{
    public class CanvasConnector : CanvasElement
    {
        //public new CanvasElementType Type => CanvasElementType.Connector;
        public string SourceShapeId { get; set; }
        public string SourceConnectionPointId { get; set; }
        public string SourceConnectionPointCase { get; set; }
        public string DestShapeId { get; set; }
        public string DestConnectionPointId { get; set; }
    }
}
