using AutoMapper;
using Domain.Entities;
using Service.DTO.Account;
using Service.DTO.Admin.AboutBannerImg;
using Service.DTO.Admin.AskUsFrom;
using Service.DTO.Admin.BlogCategory;
using Service.DTO.Admin.BlogPost;
using Service.DTO.Admin.Brand;
using Service.DTO.Admin.Category;
using Service.DTO.Admin.Color;
using Service.DTO.Admin.Instagram;
using Service.DTO.Admin.LandingBanner;
using Service.DTO.Admin.Sliders;
using Service.DTO.Admin.SubscribeImg;
using Service.DTO.Admin.Tag;
using Service.DTOs.Admin.Settings;
using Service.DTOs.UI.Wishlists;

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

            CreateMap<BlogCategory, BlogCategoryDto>();
            CreateMap<BlogCategoryCreateDto, BlogCategory>();
            CreateMap<BlogCategoryEditDto, BlogCategory>();

            CreateMap<BlogPost, BlogPostDto>().ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)); 
            CreateMap<BlogPostImg, BlogPostImgDto>(); 
            CreateMap<BlogPostCreateDto, BlogPost>().ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<BlogPostEditDto, BlogPost>().ForMember(dest => dest.Images, opt => opt.Ignore());


            CreateMap<Category,CategoryDto>();
            CreateMap<CategoryCreateDto,Category>();
            CreateMap<CategoryEditDto,Category>().ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Brand,BrandDto>();
            CreateMap<BrandCreateDto,Brand>();
            CreateMap<BrandEditDto,Brand>().ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Tag,TagDto>();
            CreateMap<TagCreateDto,Tag>();
            CreateMap<TagEditDto,Tag>();
                     
            CreateMap<Color,ColorDto>();
            CreateMap<ColorCreateDto,Color>();
            CreateMap<ColorEditDto,Color>();

            CreateMap<Wishlist, WishlistDto>();
            CreateMap<WishlistDto, Wishlist>();
            CreateMap<WishlistProduct, WishlistProductDto>();
            CreateMap<WishlistProductDto, WishlistProduct>();

            CreateMap<Setting, SettingDto>();
            CreateMap<SettingCreateDto, Setting>();
            CreateMap<SettingEditDto, Setting>();
        }
    }
}
