using System.Linq;
using CNTK;
using CourseProject.Core.DataAccess.Models;
using CourseProject.Engine.Cntk.Extensions;
using CourseProject.Engine.Interfaces;
using CourseProject.Engine.Models;

namespace CourseProject.Engine.Cntk
{
    public class CntkEngine : IEngine
    {
        private Function _cntkFunc;
        
        public CntkEngine(IEngineInitializer initializer) =>
            _cntkFunc = Function.Load(initializer.ModelPath, DeviceDescriptor.CPUDevice, ModelFormat.ONNX);

        public Genre PredictGenre(SongEngineModel song) => BuildOutput(_cntkFunc.Predict(BuildInput(song)));

        private float[] BuildInput(SongEngineModel song) => song.Features;

        private Genre BuildOutput(float[] output) => (Genre)output.ToList().IndexOf(output.Max());
    }
}