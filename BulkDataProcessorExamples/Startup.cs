using BulkDataProcessorExamples.EntityFramework;
using Loki.BulkDataProcessor.DependancyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace BulkDataProcessorExamples
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
            services.AddControllers();
            
            // Add file logging
            // Create a logger factory for the bulk processor
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFile("Logs/bulkprocessor-{Date}.txt", fileSizeLimitBytes: 10 * 1024 * 1024);
            });
            
            // Pass the logger to the bulk data processor
            services.AddLokiBulkDataProcessor(
                Configuration.GetConnectionString("BlogsDb"), 
                Assembly.GetExecutingAssembly(),
                loggerFactory);
                
            services.AddDbContext<BlogDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("BlogsDb")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
