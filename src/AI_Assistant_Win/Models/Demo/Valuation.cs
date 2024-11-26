using SQLite;
using System;

namespace AI_Assistant_Win.Models.Demo
{
    [Table("Valuation")]
    public class Valuation
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Indexed]
        [Column("stock_id")]
        public int StockId { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

        [Column("price")]
        public decimal Price { get; set; }
    }
}
