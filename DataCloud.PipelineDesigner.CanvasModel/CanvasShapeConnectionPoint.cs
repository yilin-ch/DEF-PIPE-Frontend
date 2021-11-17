using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.CanvasModel
{
    public enum CanvasConnectionPointType
    {
        Input = 0,
        Output = 1
    }

    public class CanvasPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class CanvasShapeConnectionPoint
    {
        public string Id { get; set; }
        public CanvasPosition Position { get; set; }
        public CanvasConnectionPointType Type { get; set; }

        public CanvasShapeConnectionPoint()
        {
            Position = new CanvasPosition();
        }
    }
}
