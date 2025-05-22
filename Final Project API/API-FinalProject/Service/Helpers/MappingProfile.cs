using AutoMapper;
using Domain.Entities;
using Service.DTO.Account;
using Service.DTO.Admin.AboutBannerImg;
using Service.DTO.Admin.AskUsFrom;
using Service.DTO.Admin.Instagram;
using Service.DTO.Admin.LandingBanner;
using Service.DTO.Admin.Sliders;
using Service.DTO.Admin.SubscribeImg;

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

            CreateMap<LandingBanner, LandingBannerDto>();
            CreateMap<LandingBannerCreateDto, LandingBanner>();
            CreateMap<LandingBannerEditDto, LandingBanner>().ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<AskUsFrom, AskUsFromDto>();
            CreateMap<AskUsFromCreateDto, AskUsFrom>();

            CreateMap<SubscribeImg, SubscribeImgDto>();
            CreateMap<SubscribeImgCreateDto, SubscribeImg>();
            CreateMap<SubscribeImgEditDto, SubscribeImg>().ForMember(dest => dest.Img, opt => opt.Ignore());

            CreateMap<Instagram, InstagramDto>();
            CreateMap<InstagramCreateDto, Instagram>();
            CreateMap<InstagramEditDto, Instagram>().ForMember(dest => dest.Img, opt => opt.Ignore());

            CreateMap<AboutBannerImg, AboutBannerImgDto>();
            CreateMap<AboutBannerImgCreateDto, AboutBannerImg>();
            CreateMap<AboutBannerImgEditDto, AboutBannerImg>().ForMember(dest => dest.Img, opt => opt.Ignore());
        }
    }
}
