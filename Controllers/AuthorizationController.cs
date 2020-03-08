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

namespace AuthorizationApi.Controllers
{
    [Route("api/[controller]/[action]")]
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
        public ActionResult<User> Register(User user)
        {

            Console.WriteLine(user);
            String password = user.Password;
            //check if account does not exist
            if (null != _userService.GetByEmail(user.Email))
            {
                return BadRequest();
            };
            // Add date time and create Hash with date as salt
            user.DateAdded = DateTime.UtcNow;
            user.Password = new PasswordHasher<String>().HashPassword(user.DateAdded.ToString(), user.Password);
            _userService.Create(user);


            user.Password = null;

            return Created("Register", user);
        }

        [HttpPost]
        [Consumes("application/json")]
        public ActionResult<UserToken> Login(UserLogin user)
        {
            UserToken userToken = _userService.Authenticate(user);
            if (userToken == null)
            {
                return NotFound();
            }

            return Ok(userToken);
        }

    }
}