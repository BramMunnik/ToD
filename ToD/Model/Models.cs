using SQLite;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ToD.Model
{
    public class Question
    {
        public string Text { get; set; }
        public bool RequiresCamera { get; set; } // Bool die aangeeft of de camera nodig is
    }

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

        public string SelectedCategories { get; set; } // Opslaan van meerdere geselecteerde categorieën in JSON-formaat

        public string QuestionPool { get; set; } // JSON voor interne vragen en opdrachten

        [Ignore]
        public List<string> Categories
        {
            get => JsonConvert.DeserializeObject<List<string>>(SelectedCategories) ?? new List<string>();
            set => SelectedCategories = JsonConvert.SerializeObject(value);
        }

        [Ignore]
        public Dictionary<string, List<string>> LocalQuestions
        {
            get => JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(QuestionPool) ?? new Dictionary<string, List<string>>();
            set => QuestionPool = JsonConvert.SerializeObject(value);
        }

        [Ignore]
        public List<string> QuestionsForSelectedCategories
        {
            get
            {
                var questions = new List<string>();
                foreach (var category in Categories)
                {
                    questions.AddRange(GetQuestionsForCategory(category));
                }
                return questions;
            }
        }

        public int DaringLevel { get; set; } // Filter op vragen afhankelijk van het niveau
        public string QRCode { get; set; }

        [Ignore]
        public List<Participant> Participants { get; set; } = new();

        private List<string> GetQuestionsForCategory(string category)
        {
            switch (category)
            {
                case "API":
                    return FetchQuestionsFromAPI(); // Vragen ophalen via API
                case "List":
                    return LocalQuestions.TryGetValue(category, out var listQuestions) ? listQuestions : new List<string>();
                case "Assignments":
                    return LocalQuestions.TryGetValue(category, out var assignments) ? assignments : new List<string>();
                default:
                    return new List<string>();
            }
        }

        private List<string> FetchQuestionsFromAPI()
        {
            // Simuleer een API-aanroep voor vragen
            return new List<string> { "API Question 1", "API Question 2", "API Question 3" };
        }
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
