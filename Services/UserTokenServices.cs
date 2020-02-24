using AuthorizationApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace AuthorizationApi.Services
{
    public class UserTokenService
    {
        private readonly IMongoCollection<UserToken> _Users;

        public UserTokenService(IAuthorizationDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Users = database.GetCollection<UserToken>(settings.UsersTokenCollectionName);
        }

        public List<UserToken> Get() =>
            _Users.Find(UserToken => true).ToList();


        public UserToken Get(string id) =>
            _Users.Find<UserToken>(UserToken => UserToken.Id == id).FirstOrDefault();

        public UserToken Create(UserToken UserToken)
        {
            _Users.InsertOne(UserToken);
            return UserToken;
        }

        public void Update(string id, UserToken UserTokenIn) =>
            _Users.ReplaceOne(UserToken => UserToken.Id == id, UserTokenIn);

        public void Remove(UserToken UserTokenIn) =>
            _Users.DeleteOne(UserToken => UserToken.Id == UserTokenIn.Id);

        public void Remove(string id) => 
            _Users.DeleteOne(UserToken => UserToken.Id == id);
    }
}