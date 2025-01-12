using System.Collections.Generic;

namespace AI_Assistant_Win.Models.Middle
{
    public class CircularAreaSummaryHistory
    {
        public CircularAreaMethodSummary Summary { get; set; }
        public List<CircularAreaMethodResult> MethodList { get; set; }
    }
}
