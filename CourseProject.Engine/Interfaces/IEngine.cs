using CourseProject.Core.DataAccess.Models;
using CourseProject.Engine.Models;

namespace CourseProject.Engine.Interfaces
{
    public interface IEngine
    {
        public Genre PredictGenre(SongEngineModel song);
    }
}