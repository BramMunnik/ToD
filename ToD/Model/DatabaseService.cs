using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToD.Model;

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
            // Zorg ervoor dat alle tabellen worden aangemaakt
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<SessionModel>().Wait();
        }

        // Algemene methodes voor data interacties
        public Task SaveItemAsync<T>(T item)
        {
            return _database.InsertAsync(item);
        }

        public Task<List<T>> GetItemsAsync<T>() where T : new()
        {
            return _database.Table<T>().ToListAsync();
        }

        public Task<int> DeleteItemAsync<T>(T item) where T : new()
        {
            return _database.DeleteAsync(item);
        }
    }
}
