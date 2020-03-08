using AuthorizationApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.IdentityModel.Tokens;
using AuthorizationApi.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _Users;
        private readonly AppSettings _appSettings;
        private readonly UserTokenService _userTokenService;

        public UserService(IAuthorizationDatabaseSettings settings, IOptions<AppSettings> appSettings, UserTokenService userTokenService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Users = database.GetCollection<User>(settings.UsersCollectionName);
            _userTokenService = userTokenService;
            _appSettings = appSettings.Value;

        }

        public UserToken Authenticate(UserLogin userLogin)
        {
            User user = GetByEmail(userLogin.Email);
            
            // return null if user not found
            if (user == null)
            {
                return null;
            }


            // return null if user password dont match
            if (new PasswordHasher<String>().VerifyHashedPassword(user.DateAdded.ToString(),user.Password,userLogin.Password) == 0)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var Expires = DateTime.UtcNow.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = Expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserToken userToken = new UserToken();
            userToken.Token = tokenHandler.WriteToken(token);
            userToken.UserId = user.Id;
            userToken.Expires = Expires;
            return _userTokenService.CreateOrUpdate(userToken);
        }


        public User GetByEmail(string email) =>
            _Users.Find<User>(User => User.Email == email).FirstOrDefault();

        public User Create(User User)
        {
            _Users.InsertOne(User);
            return User;
        }

        public void Update(string id, User UserIn) =>
            _Users.ReplaceOne(User => User.Id == id, UserIn);

        public void Remove(User UserIn) =>
            _Users.DeleteOne(User => User.Id == UserIn.Id);

        public void Remove(string id) =>
            _Users.DeleteOne(User => User.Id == id);
    }
}