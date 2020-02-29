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

            // if (null == _userService.GetByEmail (user.Email)) {
            //     return NotFound ();
            // };
            user.DateAdded = DateTime.Now;
            user.Password = new PasswordHasher<String>().HashPassword(user.DateAdded.ToString(),user.Password);
            _userService.Create(user);
            
            /*TODO
             * hash password
             * save to db
             * generate JWT => save to db
             * response 201 with JWT
             */
            // _userService.Create(user);
            // return Ok(user);
            return Created ("Register", user);
            // return CreatedAtRoute("Register", new { id = user.Id.ToString() }, user);
        }

        [HttpPost]
        [Consumes ("application/json")]
        public ActionResult<UserLogin> Login (UserLogin user) {
            Console.WriteLine ("In Login");
            Console.WriteLine (user);

            /*TODO
             * check if email exist => return error
             * check if hash(passsword) match => return error
             * generate JWT => save to db
             * response 200 with JWT
             */
            // _userService.Create(user);
            user.Password = null;
            return Ok (user);
        }

    }
}