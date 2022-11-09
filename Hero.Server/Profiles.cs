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

            this.CreateMap<Skilltree, SkilltreeResponse>();
            this.CreateMap<Skilltree, SkilltreeOverviewResponse>()
                .ForMember(dst => dst.NodeCount, src => src.MapFrom(tree => tree.Nodes.Count));
            this.CreateMap<SkilltreeNode, SkilltreeNodeResponse>();
            this.CreateMap<SkilltreeNodeRequest, SkilltreeNode>();
            this.CreateMap<CreateSkilltreeRequest, Skilltree>();

            this.CreateMap<BlueprintRequest, Blueprint>();
            this.CreateMap<Blueprint, BlueprintOverviewResponse>()
                .ForMember(dst => dst.NodeCount, src => src.MapFrom(print => print.Nodes.Count));
            this.CreateMap<Blueprint, BlueprintResponse>();
            this.CreateMap<BlueprintNode, BlueprintNodeResponse>();
            this.CreateMap<BlueprintNodeRequest, BlueprintNode>();

            this.CreateMap<User, UserResponse>();
            this.CreateMap<Group, GroupResponse>();
        }
    }
}
