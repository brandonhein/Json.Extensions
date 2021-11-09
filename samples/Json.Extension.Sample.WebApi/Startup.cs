using Hein.Swagger;
using Json.Extension.Sample.WebApi.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Json.Extension.Sample.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                options.InputFormatters.Add(new RawRequestBodyFormatter());
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info
                {
                    Title = "Json.Extensions.Sample.WebApi",
                    Version = "sample",
                    Description = "WebApi to show Json.Extensions in action",
                });

                x.AddGithubRepository("https://github.com/brandonhein/Json.Extensions");
                x.EnableAnnotations();
                x.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Json.Extensions.Sample.WebApi");

                c.DisplayRequestDuration();
                c.DocumentTitle = "Json.Extensions.Sample.WebApi - Swagger";
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
