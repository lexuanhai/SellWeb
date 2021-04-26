using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository
{
    public class AppDataBaseContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDataBaseContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<ProductCategory> ProductCategorys { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImages> ProductImagess { get; set; }
        public DbSet<ProductQuantity> ProductQuantitys { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
                .HasKey(x => x.Id);

            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
                .HasKey(x => new { x.RoleId, x.UserId });

            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
               .HasKey(x => new { x.UserId });
            // base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppRole>().ToTable("AppRole");
            modelBuilder.Entity<Function>().ToTable("Function");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<AppUserRole>().ToTable("AppUserRole");
            modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory");
            modelBuilder.Entity<Color>().ToTable("Color");
            modelBuilder.Entity<Size>().ToTable("Size");
            modelBuilder.Entity<ProductSize>().ToTable("ProductSize");
            modelBuilder.Entity<ProductColor>().ToTable("ProductColor");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<ProductImages>().ToTable("ProductImages");
            modelBuilder.Entity<ProductQuantity>().ToTable("ProductQuantity");
            modelBuilder.Entity<Bill>().ToTable("Bill");
            modelBuilder.Entity<BillDetail>().ToTable("BillDetail");

            modelBuilder.Entity<AppUser>().ToTable("AppUser");


            //modelBuilder.Entity<IdentityUserLogin>().ToTable("AppUserLogins").HasKey(x => x.UserId);
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    #region Identity Config

        //    builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

        //    builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
        //        .HasKey(x => x.Id);

        //    builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

        //    builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
        //        .HasKey(x => new { x.RoleId, x.UserId });

        //    builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
        //       .HasKey(x => new { x.UserId });

        //    #endregion Identity Config

        //}

    }
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDataBaseContext>
    {
        public AppDataBaseContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<AppDataBaseContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new AppDataBaseContext(builder.Options);
        }
    }
}
