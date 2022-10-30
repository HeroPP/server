using AutoMapper;

using Hero.Server.Core.Models;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;
using System.Linq;
using Attribute = Hero.Server.Core.Models.Attribute;

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
            //this.CreateMap<CreateSkillRequest, Skill>().ForMember(dst => dst.AttributeSkills, src => src.MapFrom(s => s.AttributeIds.ForEach(aID => new AttributeSkill(aID, s.SkillID))));
            this.CreateMap<NodeTree, NodeTreeResponse>();
            this.CreateMap<NodeTree, NodeTreeOverviewResponse>();
            //?!this.CreateMap<CreateRaceRequest, Race>().ForMember(dst => dst.AttributeRaces, src => src.MapFrom(r => r.AttributeIds));
            this.CreateMap<Attribute, AttributeResponse>();
            this.CreateMap<CreateAttributeRequest, Attribute>();
            this.CreateMap<CreateNodeTreeRequest, NodeTree>();
            this.CreateMap<User, UserResponse>();
            this.CreateMap<Group, GroupResponse>();
        }
    }
}
