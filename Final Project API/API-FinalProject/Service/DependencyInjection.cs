using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;
using Service.Services.Interfaces;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<ILandingBannerService, LandingBannerService>();
            services.AddScoped<IAskUsFromService, AskUsFromService>();
            services.AddScoped<ISubscribeImgService, SubscribeImgService>();
            services.AddScoped<IInstagramService, InstagramService>();
            
            return services;
        }
    }
}
