using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthorizationApi.Models
{
    
    [BsonIgnoreExtraElements]
    public class UserToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("token")]
        public string Token { get; set; }

        [BsonElement("timeExpires")]
        public int TimeExpires { get; set; }
    }

}