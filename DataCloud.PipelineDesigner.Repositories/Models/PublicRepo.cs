using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
namespace DataCloud.PipelineDesigner.Repositories.Models
{
    public class PublicRepo
    {

        [BsonElement("_id")]
        [BsonId]
        [Newtonsoft.Json.JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string User { get; set; }
        public string WorkflowName { get; set; }


        public PublicRepo()
        {
            Id = ObjectId.GenerateNewId().ToString();

        }
    }
}
