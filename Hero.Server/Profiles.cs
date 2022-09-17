using AutoMapper;

using Hero.Server.Core.Models;
using Hero.Server.Messages.Requests;

namespace Hero.Server
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            this.CreateMap<CreateCharacterRequest, Character>();
        }
    }
}
