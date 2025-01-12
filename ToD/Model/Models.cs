using SQLite;
using System;

namespace ToD.Model
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }

        public string Name { get; set; }

        public Guid TemporaryId { get; set; } = Guid.NewGuid();
    }

    public class SessionModel
    {
        [PrimaryKey]
        public Guid SessionID { get; set; }

        [NotNull]
        public Guid HostID { get; set; }

        [Indexed]
        public int HostUserId { get; set; }

        [Ignore]
        public User HostUser { get; set; }
    }
}
