using GamingStore.GamingStore.Models.Requests;
using AutoMapper;
using GamingStore.GamingStore.Models.Models;

namespace GamingStore.AutoMapper
{
    public class AutoMapping: Profile
    {
        public AutoMapping() 
        {
            CreateMap<AddGameRequest, Games>();
        }
    }
}
