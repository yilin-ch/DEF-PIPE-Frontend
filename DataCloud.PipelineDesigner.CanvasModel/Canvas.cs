using System;
using System.Collections.Generic;

namespace DataCloud.PipelineDesigner.CanvasModel
{
    public class Canvas
    {
        public string Name { get; set; }
        public List<CanvasElement> Elements { get; set; }

        public Canvas()
        {
            Elements = new List<CanvasElement>();
        }
    }
}
