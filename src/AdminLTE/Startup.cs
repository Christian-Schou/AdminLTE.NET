﻿using AdminLTE.Data;
using AdminLTE.Models;
using AdminLTE.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AdminLTE;

public class Startup
{
    public Startup(IWebHostEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{Environments.Development}.json", true);

        if (env.IsDevelopment())
        {
            // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
            builder.AddUserSecrets<Startup>();
        }

        builder.AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add framework services.
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
        services.AddScoped<SignInManager<ApplicationUser>, AuditableSignInManager<ApplicationUser>>();

        var mvcBuilder = services.AddMvc(config =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            config.Filters.Add(new AuthorizeFilter(policy));
        });

        services.Configure<CookieAuthenticationOptions>(options =>
        {
            options.LoginPath = new PathString("/Account/Login");
        });

        // Add application services.
        services.AddTransient<IEmailSender, AuthMessageSender>();
        services.AddTransient<ISmsSender, AuthMessageSender>();

        services.AddControllersWithViews().AddRazorRuntimeCompilation();
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
        }

        app.UseStatusCodePagesWithRedirects("~/Home/Error/{0}");

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        //Add middleware here
        app.UseMiddleware<RequestLoggingMiddleware>();


        // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

        //do not add middleware here (it wont be invoked)
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });


        //var logger = loggerFactory.CreateLogger(env.EnvironmentName);
        //app.Use(async (context, next) =>
        //{
        //    await next.Invoke();
        //});

        //app.Run(async context =>
        //{
        //    await context.Response.WriteAsync("ill be executed after the code above!");
        //    Debug.WriteLine("invoke thru await next.Invoke();");
        //});

        // Populate default user admin
        DataSeed.Seed(app.ApplicationServices).Wait();
    }
}