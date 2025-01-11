using System.ComponentModel;

namespace AI_Assistant_Win.Models.Enums
{
    public enum CircularPositionKind
    {
        [Description("上表面OP")]
        UPPER_SURFACE_OP = 1,
        [Description("上表面CE")]
        UPPER_SURFACE_CE = 2,
        [Description("上表面DR")]
        UPPER_SURFACE_DR = 3,
        [Description("下表面OP")]
        LOWER_SURFACE_OP = 4,
        [Description("下表面CE")]
        LOWER_SURFACE_CE = 5,
        [Description("下表面DR")]
        LOWER_SURFACE_DR = 6
    }
}
