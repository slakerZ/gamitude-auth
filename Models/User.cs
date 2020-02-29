using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthorizationApi.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("dateAdded")]
        public DateTime DateAdded { get; set; }
    }


    public class UserLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

    }
}