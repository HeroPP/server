using AutoMapper;

using Hero.Server.Core.Models;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;

namespace Hero.Server
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            this.CreateMap<CreateCharacterRequest, Character>();
            this.CreateMap<Character, CreateCharacterResponse>();
            this.CreateMap<Character, CharacterOverviewResponse>();
            this.CreateMap<Ability, AbilityResponse>();
            this.CreateMap<CreateAbilityRequest, Ability>();
            this.CreateMap<Skill, SkillResponse>();
            this.CreateMap<CreateSkillRequest, Skill>();
            this.CreateMap<NodeTree, NodeTreeResponse>();
            this.CreateMap<NodeTree, NodeTreeOverviewResponse>();
            this.CreateMap<CreateNodeTreeRequest, NodeTree>();

            this.CreateMap<User, UserResponse>();
            this.CreateMap<Group, GroupResponse>();
        }
    }
}
