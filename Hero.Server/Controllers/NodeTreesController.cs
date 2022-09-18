using AutoMapper;

using Hero.Server.Core.Models;
using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    [ApiController, Authorize(Roles = RoleNames.User), Route("api/[controller]")]
    public class NodeTreesController : HeroControllerBase
    {
        private readonly INodeTreeRepository repository;
        private readonly IMapper mapper;

        public NodeTreesController(INodeTreeRepository repository, IMapper mapper, ILogger<NodeTreesController> logger)
            : base(logger)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetNodeTreeByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                NodeTree? nodeTree = await this.repository.GetNodeTreeByIdAsync(id);
                if (nodeTree != null)
                {
                    return this.Ok(this.mapper.Map<NodeTreeResponse>(nodeTree));
                }

                return this.BadRequest();
            });
        }

        [HttpGet("{charId}")]
        public Task<IActionResult> GetNodeTreeOverviewsAsync(Guid charId)
        {
            return this.HandleExceptions(async () =>
            {
                List<NodeTree> nodeTrees = (await this.repository.GetAllNodeTreesOfCharacterAsync(charId)).ToList();

                return this.Ok(nodeTrees.Select(nodeTree => this.mapper.Map<NodeTreeOverviewResponse>(nodeTree)).ToList());
            });
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteNodeTreeAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteNodeTreeAsync(id);
                return this.Ok();
            });
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateNodeTreeAsync(Guid id, [FromBody] CreateNodeTreeRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                NodeTree nodeTree = this.mapper.Map<NodeTree>(request);
                await this.repository.UpdateNodeTreeAsync(id, nodeTree);
                return this.Ok();
            });
        }

        [HttpPost]
        public Task<IActionResult> CreateNodeTreeAsync([FromBody] CreateNodeTreeRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                NodeTree nodeTree = this.mapper.Map<NodeTree>(request);
                await this.repository.CreateNodeTreeAsync(nodeTree);

                return this.Ok(this.mapper.Map<NodeTreeResponse>(nodeTree));
            });
        }
    }
}
