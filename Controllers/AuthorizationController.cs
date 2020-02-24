using AuthorizationApi.Models;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UsersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _projectService;

        public UsersController(UserService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get() => _projectService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var project = _projectService.Get(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [HttpPost]
        public ActionResult<User> Create(User project)
        {
            _projectService.Create(project);

            return CreatedAtRoute("GetUser", new { id = project.Id.ToString() }, project);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, User projectIn)
        {
            var project = _projectService.Get(id);

            if (project == null)
            {
                return NotFound();
            }

            _projectService.Update(id, projectIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var project = _projectService.Get(id);

            if (project == null)
            {
                return NotFound();
            }

            _projectService.Remove(project.Id);

            return NoContent();
        }
    }
}