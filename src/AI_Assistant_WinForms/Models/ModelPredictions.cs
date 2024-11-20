using Microsoft.ML.Data;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AI_Assistant_WinForms.Models
{
    public class ModelPredictions
    {
        [ColumnName("output0")]
        public float[] Blackness { get; set; }
    }
}
