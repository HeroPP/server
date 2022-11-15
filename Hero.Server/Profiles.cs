using AutoMapper;

using Hero.Server.Core.Models;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;
using JCurth.Core.Extensions;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using Attribute = Hero.Server.Core.Models.Attribute;

namespace Hero.Server
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            this.CreateMap<CharacterRequest, Character>();
            this.CreateMap<Character, CreateCharacterResponse>();
            this.CreateMap<Character, CharacterOverviewResponse>();
            this.CreateMap<Character, CharacterDetailResponse>()
                .ForMember(dst => dst.FullSkilltrees, src => src.MapFrom(character => character.Skilltrees));

            this.CreateMap<Ability, AbilityResponse>();
            this.CreateMap<AbilityRequest, Ability>();
            this.CreateMap<SkillRequest, Skill>()
                .ForMember(dst => dst.Attributes, src => src.MapFrom(req => req.Attributes));
            this.CreateMap<AttributeSkillValueRequest, AttributeSkill>();
            this.CreateMap<Skill, SkillResponse>();
            this.CreateMap<AttributeSkill, AttributeValueResponse>();
            this.CreateMap<SkillRequest, Skill>();

            this.CreateMap<Skilltree, SkilltreeResponse>();
            this.CreateMap<Skilltree, SkilltreeOverviewResponse>()
                .ForMember(dst => dst.NodeCount, src => src.MapFrom(tree => tree.Nodes.Count));
            this.CreateMap<SkilltreeNode, SkilltreeNodeResponse>();
            this.CreateMap<SkilltreeNodeRequest, SkilltreeNode>();
            this.CreateMap<SkilltreeRequest, Skilltree>();
            this.CreateMap<Attribute, AttributeResponse>()
                .ForMember(dst => dst.IsGlobal, src => src.MapFrom(x => x.GroupId == new Guid()));

            this.CreateMap<AttributeRequest, Attribute>();

            this.CreateMap<RaceRequest, Race>()
                .ForMember(dst => dst.Attributes, src => src.MapFrom(req => req.Attributes));
            this.CreateMap<AttributeRaceValueRequest, AttributeRace>();
            this.CreateMap<AttributeRace, AttributeValueResponse>();
            this.CreateMap<Race, RaceResponse>();

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
