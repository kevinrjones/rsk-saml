using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rsk.AspNetCore.Authentication.Saml2p;

namespace sp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "cookie";
                    options.DefaultChallengeScheme = "idp1";
                })
                .AddCookie("cookie")
                .AddSaml2p("idp1", options => {
                    options.Licensee = "DEMO";
                    options.LicenseKey = "eyJTb2xkRm9yIjowLjAsIktleVByZXNldCI6NiwiU2F2ZUtleSI6ZmFsc2UsIkxlZ2FjeUtleSI6ZmFsc2UsIlJlbmV3YWxTZW50VGltZSI6IjAwMDEtMDEtMDFUMDA6MDA6MDAiLCJhdXRoIjoiREVNTyIsImV4cCI6IjIwMjAtMDItMjdUMDA6MDA6MDAiLCJpYXQiOiIyMDIwLTAxLTI3VDA4OjM3OjQ0Iiwib3JnIjoiREVNTyIsImF1ZCI6Mn0=.RxgEpPJbDNr+y7F4D0tsljGEzYcoF1yk+4/54+QYpFaAl/v8s8Ru/p2/cJ2AicN31Ocyze1JGrHwNKxWZ2jHtK/S59Zh1r0o06SlXNzVHwhxs7D0vyAxN8Ma1a36EIaxgDx77qmxo45y3t6avoTvWYUk75QZepoy+0YGL5znwE1HjHEymGKduMWAyPjPyJR3zGGn4x3Fg7uOuAgYyE4y8EsI7B0gNG0UVhXtb8IVhSLVhpS/k8eoFNCko9/IQZCfCwrrm0YWHJveT4WYNoMA7oioK/dJinzRViAGdN6UvDmwS4vyDwUupenrXG3y3EIk5/HO0BlW/eCBH8PSSxbEQnXtBQCQHuTKPx0aHFnOJe21OwXs9PsxZzGdLjh97O0IWotQtwHJfb24P7DDWmizkSQPwA+C2GQCK8/MV9K8Rk5sZ29v6nEiYncDTQtgm/Jrdz5cOWrr9QEzA4P4nXk5mry8t2Kaw59t0O9VnPMgIikGcv4IbK3fz5tSRe2pjuJpRyWYJNJHszI3NkxAtOop8txBjDPx4GMMbNg5pkMsOP3JXA38+tmcejV/xU/2+lq41sD9y0IKM0kD0jpermV4Hz8H9ldI1z1SNTjS20NIJnGLe/2ltIBVOGoq7x6Blcebnei24bSLaAnKfLc3UEa3QG5P1wLEaRj2NOd/mAz3KQs=";

                    options.IdentityProviderOptions = new IdpOptions
                    {
                        EntityId = "http://localhost:5000",
                        SigningCertificate = new X509Certificate2("idsrv3test.cer"),
                        SingleSignOnEndpoint = new SamlEndpoint("http://localhost:5000/saml/sso", SamlBindingTypes.HttpRedirect),
                        SingleLogoutEndpoint = new SamlEndpoint("http://localhost:5000/saml/slo", SamlBindingTypes.HttpRedirect),
                    };

                    options.ServiceProviderOptions = new SpOptions
                    {
                        EntityId = "http://localhost:5002/saml",
                        MetadataPath = "/saml/metadata",
                        SignAuthenticationRequests = false
                    };

                    options.NameIdClaimType = "sub";
                    options.CallbackPath = "/signin-saml-1";
                    options.SignInScheme = "cookie";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}