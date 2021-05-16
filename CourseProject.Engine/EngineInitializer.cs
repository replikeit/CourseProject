using CourseProject.Engine.Interfaces;
using CourseProject.Engine.Models;
using Microsoft.Extensions.Configuration;

namespace CourseProject.Engine
{
    public class EngineInitializer : IEngineInitializer
    {
        public EngineInitializer(IConfiguration configuration)
        {
            ModelPath = configuration.GetSection("Engine")["ModelPath"];
        }
        
        public string ModelPath { get; set; }
    }
}