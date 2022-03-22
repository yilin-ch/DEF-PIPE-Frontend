using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.CanvasModel
{
    public enum CanvasElementType
    {
        Shape = 0,
        Connector = 1
    }
    public enum CanvasElementPropertyType
    {
        SingleLineText = 0,
        MultiLineText = 1,
        Select = 2,
    }
}
