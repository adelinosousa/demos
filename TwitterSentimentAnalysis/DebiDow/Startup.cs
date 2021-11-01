using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DebiDow.Application.UseCases.AnalyzeSentiment;
using DebiDow.Integration.UseCases.AnalyzeSentiment;
using DebiDow.Application.UseCases.SearchTwitter;
using DebiDow.Integration.UseCases.SearchTwitter;

namespace DebiDow
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private IConfiguration configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAnalyzeSentimentRepository, AnalyzeSentimentRepository>();
            services.AddTransient<IAnalyzeSentimentInteractor, AnalyzeSentimentInteractor>();

            services.AddTransient<ISearchTwitterRepository, SearchTwitterRepository>();
            services.AddTransient<ISearchTwitterInteractor, SearchTwitterInteractor>();

            services.AddSingleton(configuration);

            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
