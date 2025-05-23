using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseEntity).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<LandingBanner> LandingBanners { get; set; }
        public DbSet<AskUsFrom> AskUsFroms { get; set; }
        public DbSet<SubscribeImg> SubscribeImgs { get; set; }
        public DbSet<Instagram> Instagrams { get; set; }      
        public DbSet<AboutBannerImg> AboutBannerImgs { get; set; } 
        public DbSet<BlogCategory> BlogCategories { get; set; } 
        public DbSet<BlogPost> BlogPosts { get; set; } 
        public DbSet<BlogPostImg> BlogPostImgs { get; set; } 

        
    }
}
