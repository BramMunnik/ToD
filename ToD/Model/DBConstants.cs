using SQLite;

namespace ToD
{
    public static class DBConstants
    {
        private const string DBFileName = "SQLiteDemo.db3";

        public const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;

        // Het pad naar de database
        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DBFileName);
    }
}