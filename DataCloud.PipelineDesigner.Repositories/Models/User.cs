using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace DataCloud.PipelineDesigner.Repositories.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public MongoDB.Bson.ObjectId _id { get; set; }
        public string Username { get; set; }
        public List<Template> Templates { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public User()
        {
            CreatedAt = DateTimeOffset.Now;
        }
    }
}
