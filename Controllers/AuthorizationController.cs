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

namespace AuthorizationApi.Controllers {
    [Route ("api/[controller]/[action]")]
    [ApiController]
    // [Authorize]
    public class AuthorizationController : ControllerBase {
        private readonly UserService _userService;
        private readonly UserTokenService _userTokenService;

        public AuthorizationController (UserService userService, UserTokenService userTokenService) {
            _userService = userService;
            _userTokenService = userTokenService;
        }

        [HttpPost]
        [Consumes ("application/json")]
        public ActionResult<User> Register (User user) {

            Console.WriteLine ("In Register");
            Console.WriteLine (user);
            String password = user.Password;
            // if (null != _userService.GetByEmail (user.Email)) {
            //     return NotFound ();
            // };
            user.DateAdded = DateTime.Now;
            user.Password = new PasswordHasher<String>().HashPassword(user.DateAdded.ToString(),user.Password);
            _userService.Create(user);
            

            user.Password = null;
            // UserLogin userLogin = new UserLogin();
            // userLogin.Email = user.Email;
            // userLogin.Password = password;

            
            // RedirectToAction("Login",userLogin);
            return Created("Register",user);
            // return CreatedAtRoute("Register", new { id = user.Id.ToString() }, user);
        }

        [HttpPost]
        [Consumes ("application/json")]
        public ActionResult<UserToken> Login (UserLogin user) {
            Console.WriteLine ("In Login");

            return Ok (_userService.Authenticate(user));
        }

    }
}