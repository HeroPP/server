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
        private readonly ILogger<NodeTreesController> logger;

        public NodeTreesController(INodeTreeRepository repository, IMapper mapper, ILogger<NodeTreesController> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public Task<Response<NodeTreeResponse?>> GetNodeTreeByIdAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                NodeTree? nodeTree = await this.repository.GetNodeTreeByIdAsync(id);
                if (nodeTree != null)
                {
                    return this.mapper.Map<NodeTreeResponse>(nodeTree);
                }

                return null;
            });
        }

        [HttpGet("{charId}")]
        public Task<Response<List<NodeTreeOverviewResponse>>> GetNodeTreeOverviewsAsync(Guid charId)
        {
            return this.HandleExceptions(async () =>
            {
                List<NodeTree> nodeTrees = (await this.repository.GetAllNodeTreesOfCharacterAsync(charId)).ToList();

                return nodeTrees.Select(nodeTree => this.mapper.Map<NodeTreeOverviewResponse>(nodeTree)).ToList();
            });
        }

        [HttpDelete("{id}")]
        public Task<Response> DeleteNodeTreeAsync(Guid id)
        {
            return this.HandleExceptions(async () =>
            {
                await this.repository.DeleteNodeTreeAsync(id);
            });
        }

        [HttpPut("{id}")]
        public Task<Response> UpdateNodeTreeAsync(Guid id, [FromBody] CreateNodeTreeRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                NodeTree nodeTree = this.mapper.Map<NodeTree>(request);
                await this.repository.UpdateNodeTreeAsync(id, nodeTree);
            });
        }

        [HttpPost]
        public Task<Response<NodeTreeResponse>> CreateNodeTreeAsync([FromBody] CreateNodeTreeRequest request)
        {
            return this.HandleExceptions(async () =>
            {
                NodeTree nodeTree = this.mapper.Map<NodeTree>(request);
                await this.repository.CreateNodeTreeAsync(nodeTree);

                return this.mapper.Map<NodeTreeResponse>(nodeTree);
            });
        }
    }
}
