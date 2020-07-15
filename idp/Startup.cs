// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Security.Cryptography.X509Certificates;
using IdentityServer4;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace idp
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddTestUsers(TestUsers.Users)
            .AddSigningCredential(new X509Certificate2("idsrv3test.pfx", "idsrv3test"))
            .AddSamlPlugin(options =>
            {
                options.Licensee = "DEMO";
                options.LicenseKey = "eyJTb2xkRm9yIjowLjAsIktleVByZXNldCI6NiwiU2F2ZUtleSI6ZmFsc2UsIkxlZ2FjeUtleSI6ZmFsc2UsIlJlbmV3YWxTZW50VGltZSI6IjAwMDEtMDEtMDFUMDA6MDA6MDAiLCJhdXRoIjoiREVNTyIsImV4cCI6IjIwMjAtMDItMjdUMDA6MDA6MDAiLCJpYXQiOiIyMDIwLTAxLTI3VDA4OjM3OjQ0Iiwib3JnIjoiREVNTyIsImF1ZCI6Mn0=.RxgEpPJbDNr+y7F4D0tsljGEzYcoF1yk+4/54+QYpFaAl/v8s8Ru/p2/cJ2AicN31Ocyze1JGrHwNKxWZ2jHtK/S59Zh1r0o06SlXNzVHwhxs7D0vyAxN8Ma1a36EIaxgDx77qmxo45y3t6avoTvWYUk75QZepoy+0YGL5znwE1HjHEymGKduMWAyPjPyJR3zGGn4x3Fg7uOuAgYyE4y8EsI7B0gNG0UVhXtb8IVhSLVhpS/k8eoFNCko9/IQZCfCwrrm0YWHJveT4WYNoMA7oioK/dJinzRViAGdN6UvDmwS4vyDwUupenrXG3y3EIk5/HO0BlW/eCBH8PSSxbEQnXtBQCQHuTKPx0aHFnOJe21OwXs9PsxZzGdLjh97O0IWotQtwHJfb24P7DDWmizkSQPwA+C2GQCK8/MV9K8Rk5sZ29v6nEiYncDTQtgm/Jrdz5cOWrr9QEzA4P4nXk5mry8t2Kaw59t0O9VnPMgIikGcv4IbK3fz5tSRe2pjuJpRyWYJNJHszI3NkxAtOop8txBjDPx4GMMbNg5pkMsOP3JXA38+tmcejV/xU/2+lq41sD9y0IKM0kD0jpermV4Hz8H9ldI1z1SNTjS20NIJnGLe/2ltIBVOGoq7x6Blcebnei24bSLaAnKfLc3UEa3QG5P1wLEaRj2NOd/mAz3KQs=";
                options.WantAuthenticationRequestsSigned = false;
            })
            .AddInMemoryServiceProviders(Config.ServiceProviders);
                

            // in-memory, code config
            builder.AddInMemoryIdentityResources(Config.Ids);
            builder.AddInMemoryApiResources(Config.Apis);
            builder.AddInMemoryClients(Config.Clients);
            
            builder.Services.Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme,
                cookie => { cookie.Cookie.Name = "idsrv.idp"; });
            
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseIdentityServerSamlPlugin();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}