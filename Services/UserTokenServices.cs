using AuthorizationApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<UserToken> CreateOrUpdateAsync(UserToken userToken)
        {
            UserToken user = await _UsersToken.Find(UserToken => UserToken.UserId == userToken.UserId).FirstOrDefaultAsync();
            if (user == null)
            {
                await _UsersToken.InsertOneAsync(userToken);
            }
            else
            {
                userToken.Id=user.Id;
                await UpdateAsync(userToken);
            }
            return userToken;
        }

        public async Task UpdateAsync(UserToken UserTokenIn) =>
            await _UsersToken.ReplaceOneAsync(UserToken => UserToken.UserId == UserTokenIn.UserId, UserTokenIn);

        public void Remove(UserToken UserTokenIn) =>
            _UsersToken.DeleteOne(UserToken => UserToken.Id == UserTokenIn.Id);

        public void Remove(string id) =>
            _UsersToken.DeleteOne(UserToken => UserToken.Id == id);
    }
}