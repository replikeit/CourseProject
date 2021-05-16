using CourseProject.Engine.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CourseProject.Engine.Extensions
{
    public static class EngineServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureEngine<TEngine>(this IServiceCollection services)
            where TEngine : class, IEngine => 
            services.AddSingleton<IEngineInitializer, EngineInitializer>().
                AddSingleton<IEngine, TEngine>();
    }
}