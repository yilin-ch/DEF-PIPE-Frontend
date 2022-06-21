using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using DataCloud.PipelineDesigner.CanvasModel;
using DataCloud.PipelineDesigner.Repositories.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Newtonsoft.Json;

namespace DataCloud.PipelineDesigner.Repositories.Models
{
    [BsonIgnoreExtraElements]
    public class Template
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonConverter(typeof(StringToObjectId))]
        public MongoDB.Bson.ObjectId _id { get; set; }
        [JsonProperty("id")]
        [BsonElement("id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public String? SourceTemplateId { get; set; }
        public dynamic CanvasTemplate { get; set; }
        public dynamic ResourceProviders { get; set; }
        public Boolean Public { get; set; }

        public Template()
        {
            _id = ObjectId.GenerateNewId();
            CreatedAt = DateTimeOffset.Now;
            ModifiedAt = DateTimeOffset.Now;
        }


    }
}
