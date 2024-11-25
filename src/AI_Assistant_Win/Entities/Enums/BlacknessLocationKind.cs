using System.ComponentModel;

namespace AI_Assistant_Win.Entities.Enums
{
    public enum BlacknessLocationKind
    {
        [Description("表面OP")]
        SURFACE_OP = 1,
        [Description("表面CE")]
        SURFACE_CE = 2,
        [Description("表面DR")]
        SURFACE_DR = 3,
        [Description("里面OP")]
        INSIDE_OP = 4,
        [Description("里面CE")]
        INSIDE_CE = 5,
        [Description("里面DR")]
        INSIDE_DR = 6
    }
}
