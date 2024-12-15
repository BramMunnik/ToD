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
            _database.CreateTableAsync<TodoItem>().Wait();
        }

        public Task<List<TodoItem>> GetItemsAsync()
        {
            return _database.Table<TodoItem>().ToListAsync();
        }

        public Task<int> SaveItemAsync(TodoItem item)
        {
            if (item.Id != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TodoItem item)
        {
            return _database.DeleteAsync(item);
        }
    }
}
