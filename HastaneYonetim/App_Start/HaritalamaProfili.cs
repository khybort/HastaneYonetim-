using AutoMapper;
using HastaneYonetim.Core.Dto;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.Repositories;

namespace HastaneYonetim.App_Start
{
    public class HaritalamaProfili : Profile
    {
        public HaritalamaProfili()
        {
            Mapper.CreateMap<Hasta, HastaDto>();
            Mapper.CreateMap<Sehir, SehirDto>();
            Mapper.CreateMap<Doktor, DoktorDto>();
            Mapper.CreateMap<Uzmanlik, UzmanlikDto>();

        }
    }
}