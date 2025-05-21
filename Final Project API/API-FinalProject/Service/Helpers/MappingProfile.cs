using AutoMapper;
using Domain.Entities;
using Service.DTO.Account;
using Service.DTO.Admin.Sliders;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, AppUser>();

            CreateMap<Slider, SliderDto>();            
            CreateMap<SliderCreateDto, Slider>();
            CreateMap<SliderEditDto, Slider>().ForMember(dest => dest.Image, opt => opt.Ignore());                      
        }
    }
}
