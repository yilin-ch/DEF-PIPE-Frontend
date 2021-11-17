using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCloud.PipelineDesigner.CanvasModel
{
    public class BaseEntity
    {
        [JsonExtensionData]
        public Dictionary<string, JToken> AdditionalProperties { get; set; } = new Dictionary<string, JToken>();

        public BaseEntity()
        {

        }
    }
}
