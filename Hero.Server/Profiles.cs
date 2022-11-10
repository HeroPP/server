using AutoMapper;

using Hero.Server.Core.Models;
using Hero.Server.Messages.Requests;
using Hero.Server.Messages.Responses;
using JCurth.Core.Extensions;
using System.Linq;
using System.Xml.Linq;
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
            this.CreateMap<Character, CharacterDetailResponse>()
                .ForMember(
                dst => dst.Attributes, src => src.MapFrom(
                    c => c.Race.AttributeRaces.Select(
                        ar => new AttributeValueResponse
                {
                    Value = ar.Value + c.Skilltrees.Where(
                        s => s.IsActiveTree).SelectMany(
                            nt => nt.Nodes.Select(
                                n => n.Skill.AttributeSkills.Where(
                                    ats => ats.AttributeId == ar.AttributeId).Select(
                                    s => s.Value).Sum())).Sum(),
                    Id = ar.AttributeId,
                    Name = ar.Attribute.Name,
                    MinValue = ar.Attribute.MinValue,
                    MaxValue = ar.Attribute.MaxValue,
                    Description = ar.Attribute.Description,
                    IconUrl = ar.Attribute.IconUrl,
                    StepSize = ar.Attribute.StepSize
                }).ToList()
                .Concat(c.Skilltrees.Where(
                    s => s.IsActiveTree).SelectMany(
                    nt => nt.Nodes.SelectMany(
                        n => n.Skill.AttributeSkills.Where(
                            ats => !c.Race.AttributeRaces.Select(
                                ar => ar.AttributeId).ToList().Contains(
                                ats.AttributeId)).DistinctBy(
                            ats => ats.AttributeId))).Select(ats => new AttributeValueResponse
                                {
                                    Id = ats.Attribute.Id,
                                    Value = c.Skilltrees.Where(
                                        s => s.IsActiveTree).SelectMany(
                                        nt => nt.Nodes.Select(
                                            n => n.Skill.AttributeSkills.Where(
                                                atsinner => atsinner.AttributeId == ats.AttributeId).Select(
                                                s => s.Value).Sum())).Sum(),
                                    Name = ats.Attribute.Name,
                                    MinValue = ats.Attribute.MinValue,
                                    MaxValue = ats.Attribute.MaxValue,
                                    Description = ats.Attribute.Description,
                                    IconUrl = ats.Attribute.IconUrl,
                                    StepSize = ats.Attribute.StepSize
                                }).ToList())
                ));

            this.CreateMap<Ability, AbilityResponse>();
            this.CreateMap<CreateAbilityRequest, Ability>();
            this.CreateMap<CreateSkillRequest, Skill>();
            this.CreateMap<AttributeSkillValueRequest, AttributeSkill>();
            this.CreateMap<Skill, SkillResponse>();
            this.CreateMap<AttributeSkill, AttributeValueResponse>()
                 .ForMember(dst => dst.Id, src => src.MapFrom(ats => ats.Attribute.Id))
                 .ForMember(dst => dst.Name, src => src.MapFrom(ats => ats.Attribute.Name))
                 .ForMember(dst => dst.IconUrl, src => src.MapFrom(ats => ats.Attribute.IconUrl))
                 .ForMember(dst => dst.Description, src => src.MapFrom(ats => ats.Attribute.Description))
                 .ForMember(dst => dst.StepSize, src => src.MapFrom(ats => ats.Attribute.StepSize))
                 .ForMember(dst => dst.MinValue, src => src.MapFrom(ats => ats.Attribute.MinValue))
                 .ForMember(dst => dst.MaxValue, src => src.MapFrom(ats => ats.Attribute.MaxValue))
                 .ForMember(dst => dst.Value, src => src.MapFrom(ats => ats.Value));
            this.CreateMap<CreateSkillRequest, Skill>();
            this.CreateMap<Skilltree, SkilltreeResponse>();
            this.CreateMap<Skilltree, SkilltreeOverviewResponse>()
                .ForMember(dst => dst.NodeCount, src => src.MapFrom(tree => tree.Nodes.Count));
            this.CreateMap<Node, NodeResponse>();
            this.CreateMap<NodeRequest, Node>();
            this.CreateMap<CreateSkilltreeRequest, Skilltree>();
            this.CreateMap<Attribute, AttributeResponse>();
            this.CreateMap<CreateAttributeRequest, Attribute>();
            this.CreateMap<CreateRaceRequest, Race>();
            this.CreateMap<AttributeRaceValueRequest, AttributeRace>();
            this.CreateMap<AttributeRace, AttributeValueResponse>()
                 .ForMember(dst => dst.Id, src => src.MapFrom(atr => atr.Attribute.Id))
                 .ForMember(dst => dst.Name, src => src.MapFrom(atr => atr.Attribute.Name))
                 .ForMember(dst => dst.IconUrl, src => src.MapFrom(atr => atr.Attribute.IconUrl))
                 .ForMember(dst => dst.Description, src => src.MapFrom(atr => atr.Attribute.Description))
                 .ForMember(dst => dst.StepSize, src => src.MapFrom(atr => atr.Attribute.StepSize))
                 .ForMember(dst => dst.MinValue, src => src.MapFrom(atr => atr.Attribute.MinValue))
                 .ForMember(dst => dst.MaxValue, src => src.MapFrom(atr => atr.Attribute.MaxValue))
                 .ForMember(dst => dst.Value, src => src.MapFrom(atr => atr.Value));
            this.CreateMap<Race, RaceResponse>();
            this.CreateMap<User, UserResponse>();
            this.CreateMap<Group, GroupResponse>();
        }
    }
}
