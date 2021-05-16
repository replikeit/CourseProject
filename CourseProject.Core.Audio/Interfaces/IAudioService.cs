using System.Threading.Tasks;
using CourseProject.Core.DataAccess.Models;

namespace CourseProject.Core.Audio.Interfaces
{
    public interface IAudioService
    {
        public Task SongProcess(Song song);
    }
}