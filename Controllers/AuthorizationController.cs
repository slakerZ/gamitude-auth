using AuthorizationApi.Models;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AuthorizationApi.Controllers
{
    [Route("api/[controller]")]
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
        [Route("Register")]
        public ActionResult<User> Register(User user)
        {
            Console.WriteLine("In Register");
            Console.WriteLine(user.ToString());

            /*TODO
            * check if email exist => return error
            * hash password
            * save to db
            * generate JWT => save to db
            * response 201 with JWT
            */
            // _userService.Create(user);

            return CreatedAtRoute("Register", user);
            // return CreatedAtRoute("Register", new { id = user.Id.ToString() }, user);
        }

        [HttpPost]
        [Route("Login")]

        public ActionResult<UserLogin> Login(UserLogin user)
        {
            Console.WriteLine(user);

            /*TODO
            * check if email exist => return error
            * check if hash(passsword) match => return error
            * generate JWT => save to db
            * response 200 with JWT
            */
            // _userService.Create(user);
            user.Password = null;
            return Ok(user);
        }


    }
}