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
            this.CreateMap<Skill, SkillResponse>();
            this.CreateMap<CreateSkillRequest, Skill>();
            this.CreateMap<Skilltree, SkilltreeResponse>();
            this.CreateMap<Skilltree, SkilltreeOverviewResponse>()
                .ForMember(dst => dst.NodeCount, src => src.MapFrom(tree => tree.Nodes.Count));
            this.CreateMap<Node, NodeResponse>();
            this.CreateMap<NodeRequest, Node>();
            this.CreateMap<CreateSkilltreeRequest, Skilltree>();
            this.CreateMap<Attribute, AttributeResponse>();
            this.CreateMap<CreateAttributeRequest, Attribute>();
            this.CreateMap<User, UserResponse>();
            this.CreateMap<Group, GroupResponse>();
        }
    }
}
