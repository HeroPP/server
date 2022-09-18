using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class UsersController : HeroControllerBase
    {
        private readonly IUserRepository repository;

        public UsersController(IUserRepository repository, ILogger<UsersController> logger)
            : base(logger)
        {
            this.repository = repository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {
            return await this.HandleExceptions(async () => this.Ok(await repository.CreateUserAsync(this.HttpContext.User.GetUserId())));
        }
    }
}
