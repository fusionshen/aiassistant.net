using System.ComponentModel;

namespace AI_Assistant_Win.Models.Enums
{
    public enum BlacknessLevelKind
    {
        [Description("极差")]
        NG_1 = 1,
        [Description("差")]
        NG_2 = 2,
        [Description("及格")]
        OK_3 = 3,
        [Description("良")]
        OK_4 = 4,
        [Description("优")]
        OK_5 = 5
    }
}
