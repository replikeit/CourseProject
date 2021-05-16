using System.Threading.Tasks;
using CourseProject.Core.Audio.Interfaces;
using CourseProject.Core.DataAccess;
using CourseProject.Core.DataAccess.Models;
using CourseProject.Engine;
using CourseProject.Engine.Interfaces;
using CourseProject.Engine.Models;

namespace CourseProject.Core.Audio
{
    public class AudioService : IAudioService
    {
        private readonly MusicDbContext _dbContext;
        private readonly IEngine _engine;

        public AudioService(MusicDbContext dbContext, IEngine engine)
        {
            _dbContext = dbContext;
            _engine = engine;
        }

        public async Task SongProcess(Song song)
        {
            var features = await MfccConverter.GetMfccAsync(song.FilePath);
            var songEngine = new SongEngineModel(){Features = features};
            
            var predictedGenre = _engine.PredictGenre(songEngine);

            song.Genre = predictedGenre;
            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();
        }
    }
}