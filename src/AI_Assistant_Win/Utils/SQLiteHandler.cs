using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Demo;
using SQLite;

namespace AI_Assistant_Win.Utils
{
    /// <summary>
    /// https://github.com/praeclarum/sqlite-net
    /// </summary>
    public class SQLiteHandler
    {

        private readonly SQLiteConnection _db;
        /// <summary>
        /// Inside of our constructor, we are creating a new instance of the SQLiteConnection 
        /// class and pass the path of our SQLite database inside the brackets 
        /// (use a mixture of strings and filesystem helpers like Xamarin.Essentials). 
        /// Then we are creating the Stock and Valuation tables. This is where the 
        /// ORM part comes in. If we run this application, the tables will be automatically 
        /// mapped to the SQLite database and we will be able to access them 
        /// using SQL queries. If the tables do not exist in the SQLite database, 
        /// they will be generated. If they do exist, any existing data in the table will 
        /// be "Migrated" and the tables will be updated (if new fields have been added).
        /// Now that the database has been successfully mapped, we can begin performing queries on the data. 
        /// Let's start by inserting a new row into the Stock table.
        /// public void AddStock() {	
        ///
        ///     var stock = new Stock{
        ///            Symbol = "MSFT"
        ///     };
        ///     
        ///     _db.Insert(stock );		
        /// }
        /// And just like that, we have inserted a new row into the Stock table. 
        /// The Primary key is automatically generated, so we do not need to specify it: 
        /// all we needed to do was create a new Stock object and pass it to _db.Insert.
        /// If we query the database, we will be able to see the new record.
        /// public void GetStocks()		
        /// {
        ///    var stocks = _db.Query<Stock>("SELECT * FROM Stocks");
        ///    
        ///    foreach(var stock in stocks)
        ///    {
        ///        Console.WriteLine(stock.Symbol);
        ///    }
        /// }
        /// </summary>
        public SQLiteHandler()
        {
            _db = new SQLiteConnection("./Resources/database.sqlite");
            _db.CreateTable<Stock>();
            _db.CreateTable<Valuation>();
            _db.CreateTable<BlacknessMethodResult>();
            _db.CreateTable<BlacknessMethodItem>();
            _db.CreateTable<CameraBinding>();
            _db.CreateTable<SystemConfig>();
            _db.CreateTable<BlacknessUploadResult>();
            _db.CreateTable<CalculateScale>();
            _db.CreateTable<CircularAreaMethodResult>();
            _db.CreateTable<CircularAreaMethodSummary>();
            _db.CreateTable<CircularAreaUploadResult>();
        }

        // 私有静态内部类，负责持有唯一实例
        private static class Nested
        {
            // 静态成员变量，在第一次使用时才创建实例
            internal static readonly SQLiteHandler instance = new();
        }

        /// <summary>
        /// 公共静态方法提供全局访问点
        /// Eager Initialization 饿汉式
        /// Lazy Initialization 懒汉式
        /// Double-Checked Locking 双重检查锁定
        /// ✔ Bill Pugh Singleton 静态内部类 这种方法利用了C#静态内部类的特性，实现了线程安全的懒加载，且无需显式锁定。
        /// </summary>
        public static SQLiteHandler Instance => Nested.instance;

        /// <summary>
        /// 公共方法用于访问私有属性
        /// var db = SQLiteHandler.Instance.GetSQLiteConnection();
        /// var stock = new Stock
        /// {
        ///    Symbol = "MSFT"
        /// };
        /// db.Insert(stock);
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection GetSQLiteConnection()
        {
            return _db;
        }
    }
}
