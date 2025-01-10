using SQLite;

namespace ToD
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            _database = new SQLiteAsyncConnection(DBConstants.DatabasePath, DBConstants.Flags);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            // Zorg ervoor dat de tabellen worden aangemaakt
            _database.CreateTableAsync<User>().Wait();
        }
    }
}
