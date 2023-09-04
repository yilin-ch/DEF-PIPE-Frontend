using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.CanvasModel
{
    public class CanvasElement: BaseEntity
    {
        [JsonProperty("type")]
        public CanvasElementType Type { get; set; }
        public string ID { get; set; }

        public CanvasElement()
        {

        }
    }
}
