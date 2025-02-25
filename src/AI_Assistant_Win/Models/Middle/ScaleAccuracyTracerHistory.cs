using System.Collections.Generic;

namespace AI_Assistant_Win.Models.Middle
{
    public class ScaleAccuracyTracerHistory
    {
        public ScaleAccuracyTracer Tracer { get; set; }
        public CalculateScale Scale { get; set; }
        public bool InUse { get; set; }
        public List<GaugeBlockMethodResult> MPEList { get; set; }
        public List<GaugeBlockMethodResult> MethodList { get; set; }
    }
}
