using System.ComponentModel;

namespace AI_Assistant_Win.Models.Enums
{
    public enum EntityStateKind
    {
        [Description("修改")]
        Update = 2,
        [Description("新增")]
        Insert = 4,
        [Description("删除")]
        Delete = 8,
    }
}
