using SQLite;
using System;
using System.Collections.Generic;

namespace ToD.Model
{
    public class User
    {
        [PrimaryKey]
        public Guid UserID { get; set; }

        [NotNull]
        public string Name { get; set; }

        // Navigation property (manueel te verwerken)
        [Ignore]
        public ICollection<Session> Sessions { get; set; }
    }

    public class Session
    {
        [PrimaryKey]
        public Guid SessionID { get; set; }

        [NotNull]
        public Guid HostID { get; set; }

        [Ignore]
        public User Host { get; set; }

        [NotNull]
        public string SelectedCategories { get; set; } // JSON string

        [NotNull]
        public int DaringLevel { get; set; }

        [NotNull]
        public string QuestionPool { get; set; } // JSON string

        public string QRCode { get; set; }

        [Ignore]
        public ICollection<Participant> Participants { get; set; }
    }

    public class Participant
    {
        [PrimaryKey]
        public Guid ParticipantID { get; set; }

        [NotNull]
        public Guid SessionID { get; set; }

        [Ignore]
        public Session Session { get; set; }

        public Guid? UserID { get; set; } // Nullable voor anonieme gebruikers

        [Ignore]
        public User User { get; set; }

        [NotNull]
        public Guid TemporaryID { get; set; }
    }

    public class GameData
    {
        [PrimaryKey]
        public Guid QuestionID { get; set; }

        [NotNull]
        public string QuestionText { get; set; }

        [NotNull]
        public string Category { get; set; }

        [NotNull]
        public int DaringLevel { get; set; }

        public Guid? CreatedBy { get; set; }

        [Ignore]
        public User Creator { get; set; }

        [NotNull]
        public string QuestionType { get; set; }

        public string PhotoLink { get; set; }
    }

    public class TemporaryStorage
    {
        [PrimaryKey]
        public Guid SessionID { get; set; }

        [Ignore]
        public Session Session { get; set; }

        public Guid? ActiveQuestionID { get; set; }

        [Ignore]
        public GameData ActiveQuestion { get; set; }

        public Guid? CurrentPlayerID { get; set; }

        [Ignore]
        public User CurrentPlayer { get; set; }

        [NotNull]
        public string TurnOrder { get; set; } // JSON array
    }
}
