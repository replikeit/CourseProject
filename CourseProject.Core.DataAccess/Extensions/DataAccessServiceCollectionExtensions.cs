using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CourseProject.Core.DataAccess.Extensions
{
    public static class DataAccessServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDataAccess(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<MusicDbContext>(opt => opt.UseSqlServer(connectionString));
    }
}