using AuthorizationApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthorizationApi.Services
{
    public class UserTokenService
    {
        private readonly IMongoCollection<UserToken> _UsersToken;

        public UserTokenService(IAuthorizationDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _UsersToken = database.GetCollection<UserToken>(settings.UsersTokenCollectionName);
        }

        public List<UserToken> Get() =>
            _UsersToken.Find(UserToken => true).ToList();


        public UserToken Get(string id) =>
            _UsersToken.Find<UserToken>(UserToken => UserToken.Id == id).FirstOrDefault();

        public UserToken CreateOrUpdate(UserToken userToken)
        {
            UserToken user = _UsersToken.Find(UserToken => UserToken.UserId == userToken.UserId).FirstOrDefault();
            if (user == null)
            {
                _UsersToken.InsertOne(userToken);
            }
            else
            {
                userToken.Id=user.Id;
                Update(userToken);
            }
            return userToken;
        }

        public void Update(UserToken UserTokenIn) =>
            _UsersToken.ReplaceOne(UserToken => UserToken.UserId == UserTokenIn.UserId, UserTokenIn);

        public void Remove(UserToken UserTokenIn) =>
            _UsersToken.DeleteOne(UserToken => UserToken.Id == UserTokenIn.Id);

        public void Remove(string id) =>
            _UsersToken.DeleteOne(UserToken => UserToken.Id == id);
    }
}