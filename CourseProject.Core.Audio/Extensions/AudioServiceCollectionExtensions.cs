using CourseProject.Core.Audio.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CourseProject.Core.Audio.Extensions
{
    public static class AudioServiceCollectionExtensions
    {
        public static IServiceCollection AddAudioService(this IServiceCollection service) =>
            service.AddScoped<IAudioService, AudioService>();
    }
}