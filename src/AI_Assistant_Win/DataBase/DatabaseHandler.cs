using AI_Assistant_Win.Entities.Demo;
using SQLite;

namespace AI_Assistant_Win.DataBase
{
    public class DatabaseHandler
    {

        private SQLiteConnection _db;

        public DatabaseHandler()
        {

            _db = new SQLiteConnection("./Resources/database.sqlite");
            _db.CreateTable<Stock>();
            _db.CreateTable<Valuation>();
        }
    }
}
