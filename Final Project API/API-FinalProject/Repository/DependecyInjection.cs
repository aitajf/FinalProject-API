using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Repository.Repositories.Interface;

namespace Repository
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<ILandingBannerRepository, LandingBannerRepository>();
            services.AddScoped<IAskUsFromRepository, AskUsFromRepository>();
            services.AddScoped<ISubscribeImgRepository, SubscribeImgRepository>();
            services.AddScoped<IInstagramRepository, InstagramRepository>();
            
            return services;
        }
    }
}
