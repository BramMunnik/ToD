using SQLite;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ToD.Model
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }

        public string Name { get; set; }

        public Guid TemporaryId { get; set; } = Guid.NewGuid();

        public string PreferencesJson { get; set; }

        [Ignore]
        public Preferences Preferences
        {
            get => JsonConvert.DeserializeObject<Preferences>(PreferencesJson) ?? new Preferences();
            set => PreferencesJson = JsonConvert.SerializeObject(value);
        }
    }

    public class Preferences
    {
        public List<string> SelectedCategories { get; set; } = new();
        public int RiskLevel { get; set; }  // Schaal van 1-5
    }

    public class SessionModel
    {
        [PrimaryKey]
        public Guid SessionID { get; set; }

        [NotNull]
        public Guid HostID { get; set; }

        public string SelectedCategories { get; set; }

        public string QuestionPool { get; set; }

        [Ignore]
        public List<string> Categories
        {
            get => JsonConvert.DeserializeObject<List<string>>(SelectedCategories) ?? new List<string>();
            set => SelectedCategories = JsonConvert.SerializeObject(value);
        }

        [Ignore]
        public List<string> Questions
        {
            get => JsonConvert.DeserializeObject<List<string>>(QuestionPool) ?? new List<string>();
            set => QuestionPool = JsonConvert.SerializeObject(value);
        }

        public int DaringLevel { get; set; }
        public string QRCode { get; set; }

        [Ignore]
        public List<Participant> Participants { get; set; } = new();
    }

    public class Participant
    {
        [PrimaryKey]
        public Guid ParticipantID { get; set; }

        [NotNull]
        public Guid SessionID { get; set; }

        public Guid? UserID { get; set; }

        public Guid TemporaryID { get; set; }
    }

    public class GameData
    {
        [PrimaryKey]
        public Guid QuestionID { get; set; }

        public string QuestionText { get; set; }
        public string Category { get; set; }
        public int DaringLevel { get; set; }
        public string QuestionType { get; set; }
        public string PhotoLink { get; set; }
    }
}
