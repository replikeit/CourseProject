using System.Collections.Generic;
using System.Linq;
using CNTK;

namespace CourseProject.Engine.Cntk.Extensions
{
    public static class CntkExtensions
    {
        public static float[] Predict(this Function function, float[] input)
        {
            var inputVariable = function.Inputs.First();
            var inputValue = Value.CreateBatch<float>(new NDShape(new SizeTVector(new []{1, 25000, 1})), input, DeviceDescriptor.CPUDevice);
            var inputDict = new Dictionary<Variable, Value>()
            {
                {inputVariable, inputValue}
            };
            
            var outputVariable = function.Output;
            var outputDict = new Dictionary<Variable, Value>()
            {
                {outputVariable, null}
            };
            
            function.Evaluate(inputDict, outputDict, DeviceDescriptor.CPUDevice);

            return outputDict[outputVariable].GetDenseData<float>(outputVariable).First().ToArray();
        }
    }
}