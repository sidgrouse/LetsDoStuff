using LetsDoStuff.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LetsDoStuff.WebApi
{
    public class Startup
    {
        public Startup()
        {
        }

        public IConfiguration Configuration { get; }

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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LdsContext>(options =>
                options.UseSqlServer(connection));
            services.AddControllers();
        }
    }
}
