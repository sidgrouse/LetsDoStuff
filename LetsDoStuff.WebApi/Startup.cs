using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.WebApi
{
    public class Startup
    {
        public Startup()
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                });
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }
    }
}
