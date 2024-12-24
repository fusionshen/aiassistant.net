using SQLite;

namespace AI_Assistant_Win.Models
{
    [Table("SystemConfigs")]
    public class SystemConfig
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("key")]
        public string Key { get; set; }

        [Column("value")]
        public string Value { get; set; }
    }
}
