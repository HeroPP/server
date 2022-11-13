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
            //this.CreateMap<Character, CharacterDetailResponse>()
            //    .ForMember(
            //    dst => dst.Attributes, src => src.MapFrom(
            //        c => c.Race.Attribute.Select(
            //            ar => new AttributeValueResponse
            //    {
            //        Value = ar.Value + c.Skilltrees.Where(
            //            s => s.IsActiveTree).SelectMany(
            //                nt => nt.Nodes.Select(
            //                    n => n.Skill.Attributes.Where(
            //                        ats => ats.AttributeId == ar.AttributeId).Select(
            //                        s => s.Value).Sum())).Sum(),
            //        AttributeId = ar.AttributeId,
            //        Name = ar.Attribute.Name,
            //        MinValue = ar.Attribute.MinValue,
            //        MaxValue = ar.Attribute.MaxValue,
            //        Description = ar.Attribute.Description,
            //        IconData = ar.Attribute.IconData,
            //        StepSize = ar.Attribute.StepSize
            //    }).ToList()
            //    .Concat(c.Skilltrees.Where(
            //        s => s.IsActiveTree).SelectMany(
            //        nt => nt.Nodes.SelectMany(
            //            n => n.Skill.Attributes.Where(
            //                ats => !c.Race.Attribute.Select(
            //                    ar => ar.AttributeId).ToList().Contains(
            //                    ats.AttributeId)).DistinctBy(
            //                ats => ats.AttributeId))).Select(ats => new AttributeValueResponse
            //                    {
            //                        AttributeId = ats.Attribute.Id,
            //                        Value = c.Skilltrees.Where(
            //                            s => s.IsActiveTree).SelectMany(
            //                            nt => nt.Nodes.Select(
            //                                n => n.Skill.Attributes.Where(
            //                                    atsinner => atsinner.AttributeId == ats.AttributeId).Select(
            //                                    s => s.Value).Sum())).Sum(),
            //                        Name = ats.Attribute.Name,
            //                        MinValue = ats.Attribute.MinValue,
            //                        MaxValue = ats.Attribute.MaxValue,
            //                        Description = ats.Attribute.Description,
            //                        IconData = ats.Attribute.IconData,
            //                        StepSize = ats.Attribute.StepSize
            //                    }).ToList())
            //    ));

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

            this.CreateMap<RaceRequest, Race>();
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
