using System;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationApi.Controllers
{
    [Route("api/auth/[controller]")]
    [Route("/")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private String version = "Gamitude Auth Alpha v0.1";

        public VersionController()
        {

        }

        [HttpGet]
        public ActionResult<String> Version()
        {

            return Created("Version", version);
        }

    }
}