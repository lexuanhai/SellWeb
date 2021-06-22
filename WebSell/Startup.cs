using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebSell.Authorization;
using WebSell.Helpers;
using WSS.Core.Domain.Entities;
using WSS.Core.Dto.AutoMapper;
using WSS.Core.Repository;
using WSS.Core.Repository.Repositories;
using WSS.Service.BillService;
using WSS.Service.ColorService;
using WSS.Service.FunctionService;
using WSS.Service.PermissionService;
using WSS.Service.ProductCategoryService;
using WSS.Service.ProductQuantityService;
using WSS.Service.ProductService;
//using WSS.Service.RoleService;
//using WSS.Service.RoleService;
using WSS.Service.SizeService;
//using WSS.Service.UserRoleService;
using WSS.Service.UserService;

namespace WebSell
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDataBaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();
            // // Services
            services.AddTransient<IUserService, UserService>();
            //services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IFunctionService, FunctionService>();
            services.AddTransient<IPermissionService, PermissionService>();
            //services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<ISizeService, SizeService>();
            services.AddTransient<IProductService, ProductService>();
            // services.AddTransient<IProductQuantityService, ProductQuantityService>();
            // services.AddTransient<IProductImagesService, ProductImagesService>();
            // services.AddTransient<IBillService, BillService>();
            // //Repository
            services.AddTransient<IUserRepository, UserRepository>();
            //services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IFunctionRepository, FunctionRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IColorRepository, ColorRepository>();
            services.AddTransient<ISizeRepository, SizeRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            // services.AddTransient<IProductImagesRepository, ProductImagesRepository>();
            // services.AddTransient<IProductQuantityRepository, ProductQuantityRepository>();
            // services.AddTransient<IBillRepository, BillRepository>();
            // services.AddMvc().AddControllersAsServices();
            //services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();
            // trả về mặc định json
            services.AddMvc().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });           
            services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<AppDataBaseContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

            });
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Login}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                   name: "admin",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
            AutoMapperConfig.Init();
        }
    }
}
