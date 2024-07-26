using JwtProjectEx.Data;
using Microsoft.EntityFrameworkCore;

namespace JwtProjectEx
{
    public static class ServiceContainer
    {
        public static void RegisterDependencies(IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                string? connectionString = configuration["ConnectionStrings:DbConnection"];
                opt.UseSqlServer(connectionString);
            });
        }
    }
}
