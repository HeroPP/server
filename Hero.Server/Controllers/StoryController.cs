using AutoMapper;

using Hero.Server.Core.Models.Storyline;
using Hero.Server.Core.Repositories;
using Hero.Server.Identity;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hero.Server.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize(Roles = RoleNames.User)]
    public class StoryController : HeroControllerBase
    {
        private readonly IStoryEntryRepository storyEntryRepository;
        private readonly IStoryBookPageRepository bookPageRepository;
        private readonly IMapper mapper;

        public StoryController(IStoryEntryRepository storyEntryRepository, IStoryBookPageRepository bookPageRepository, IMapper mapper, ILogger<StoryController> logger) : base(logger)
        {
            this.storyEntryRepository = storyEntryRepository;
            this.bookPageRepository = bookPageRepository;
            this.mapper = mapper;
        }

        private StoryEntry ConvertEntry(StoryEntryRequest request)
        {
            StoryEntry entry;
            switch (request.Type)
            {
                case nameof(StoryImage):
                    entry = this.mapper.Map<StoryImage>(request);
                    break;
                case nameof(StoryBook):
                    entry = this.mapper.Map<StoryBook>(request);
                    break;
                default:
                    entry = new StoryEntry();
                    break;
            }

            return entry;
        }

        [ApiExplorerSettings(IgnoreApi = true), NonAction, Route("/error")]
        public IActionResult HandleError() => this.HandleErrors();

        [HttpGet]
        public async Task<IActionResult> GetAllStoryEntriesAsync(CancellationToken cancellationToken = default)
        {
            List<StoryEntry> entries = await this.storyEntryRepository.GetAllAsync(!this.HttpContext.User.IsInRole(RoleNames.Administrator), cancellationToken);

            return this.Ok(entries.Select(entry => this.mapper.Map<StoryEntryOverviewResponse>(entry)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoryEntryByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            StoryEntry? entry = await this.storyEntryRepository.GetByIdAsync(id, !this.HttpContext.User.IsInRole(RoleNames.Administrator), cancellationToken);

            if (null != entry)
            {
                return this.Ok(this.mapper.Map<StoryEntryResponse>(entry));
            }

            return this.NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStoryEntryAsync([FromBody] StoryEntryRequest request, CancellationToken cancellationToken = default)
        {
            StoryEntry entry = this.ConvertEntry(request);

            await this.storyEntryRepository.CreateAsync(entry, cancellationToken);

            return this.Ok(this.mapper.Map<StoryEntryOverviewResponse>(entry));
        }

        [HttpPost("reorder")]
        public async Task<IActionResult> ReorderStoryEntryAsync([FromBody] ReorderRequest request, CancellationToken cancellationToken = default)
        {
            await this.storyEntryRepository.UpdatePositionAsync(request.EntityId, request.Position, cancellationToken);

            return this.Ok();
        }

        [HttpPost("{id}/unlock"), Authorize(Roles = RoleNames.Administrator)]
        public async Task<IActionResult> SwitchActiveStateForSkilltreeAsync(Guid id, [FromBody] ChangeStateRequest request, CancellationToken cancellationToken)
        {
            await this.storyEntryRepository.UnlockAsync(id, request.State, cancellationToken);
            return this.Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStoryEntryAsync(Guid id, [FromBody] StoryEntryRequest request, CancellationToken cancellationToken = default)
        {
            StoryEntry entry = this.ConvertEntry(request);

            await this.storyEntryRepository.UpdateAsync(id, entry, cancellationToken);

            return this.Ok(this.mapper.Map<StoryEntryOverviewResponse>(entry));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoryEntryAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await this.storyEntryRepository.DeleteAsync(id, cancellationToken);

            return this.Ok();
        }
    }
}
