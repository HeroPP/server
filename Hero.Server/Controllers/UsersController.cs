using AutoMapper;

using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class UsersController : HeroControllerBase
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository repository, IMapper mapper, ILogger<UsersController> logger)
            : base(logger)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetUser()
        {
            return await this.HandleExceptions(async () => this.Ok(this.mapper.Map<UserResponse>(await this.repository.GetUserByIdAsync(this.HttpContext.User.GetUserId()))));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {
            return await this.HandleExceptions(async () => this.Ok(await repository.CreateUserIfNotExistAsync(this.HttpContext.User.GetUserId())));
        }

    }
}
