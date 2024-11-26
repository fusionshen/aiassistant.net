using SQLite;

namespace AI_Assistant_Win.Entities.Demo
{
    [Table("Stocks")]
    public class Stock
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("symbol")]
        public string Symbol { get; set; }
    }
}
