using AutoMapper;
using SimpleArchitectureCore.DTO;
using SimpleArchitectureEntities.Models;

namespace SimpleArchitectureWeb.Mappings
{
    public class AdministrationMappingProfile : Profile
    {
        public AdministrationMappingProfile()
        {
            CreateMap<GroupDto, Group>().ReverseMap();
        }
    }
}
