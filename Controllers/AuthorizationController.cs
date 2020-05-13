using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using AuthorizationApi.Models;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationApi.Controllers
{
    [Route("api/auth/[controller]/[action]")]
    [ApiController]
    // [Authorize]
    public class AuthorizationController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UserTokenService _userTokenService;

        public AuthorizationController(UserService userService, UserTokenService userTokenService)
        {
            _userService = userService;
            _userTokenService = userTokenService;
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<User>> Register(User user)
        {

            Console.WriteLine(user);
            String password = user.Password;
            //check if account does not exist
            if (null != await _userService.GetByEmailAsync(user.Email))
            {
                return BadRequest();
            };
            // Add date time and create Hash with date as salt
            user.DateAdded = DateTime.UtcNow;
            user.Password = new PasswordHasher<String>().HashPassword(user.DateAdded.ToString(), user.Password);
            user = await _userService.CreateAsync(user);

            HttpClient client = new HttpClient();
            String jsonString = "{ \"UserId\":\""+user.Id+"\"}";
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            // var result = await client.PostAsync("http://localhost:5030/api/stats/UserRank/Create", content);
            var result = await client.PostAsync("http://gamitude.rocks/api/stats/UserRank/Create", content);
            user.Password = null;
            if(result.StatusCode == HttpStatusCode.OK)
                return Created("Register", user);
            else
            {
                Console.WriteLine("error while creaing user rank");
                throw new Exception("creating user rank failed");
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<UserToken>> Login(UserLogin user)
        {
            UserToken userToken = await _userService.AuthenticateAsync(user);
            if (userToken == null)
            {
                return NotFound();
            }

            return Ok(userToken);
        }

    }
}
