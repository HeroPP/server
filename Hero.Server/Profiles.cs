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
        }
    }
}
