using EntityFrameworkCore.EncryptColumn.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Entities;
using Project.DAL.Entities.Identity;
using System.Reflection;
using System.Reflection.Emit;

namespace Project.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role,string>
    {
        private readonly IEncryptionProvider encryptionProvider;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //encryptionProvider = new GenerateEncryptionProvider("2dbe2ed97c3b42c3a7a95696a1f9f924f1e948a09adf4ee1af9b369b0b4d5b65");
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserImage> UserImage { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<RealEstateAd> RealEstateAds { get; set; }
        public DbSet<ClientInfo> Client { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //builder.UseEncryption(encryptionProvider);



        }
    }
}
