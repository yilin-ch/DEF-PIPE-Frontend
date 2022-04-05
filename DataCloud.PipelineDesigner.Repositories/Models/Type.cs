using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataCloud.PipelineDesigner.Repositories.Models
{
    public class Type
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; }
        public string Name { get; set; }


        public Type()
        {

        }
    }
}
