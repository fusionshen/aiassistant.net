using Microsoft.ML.Data;

namespace AI_Assistant_WinForms.Models
{
    public class ModelPredictions
    {
        [ColumnName("output0")]
        public float[] Blackness { get; set; }
    }
}
