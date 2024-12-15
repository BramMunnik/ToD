using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToD
{
    public class Users
    {
        public class User
        {
            public string Name { get; set; }
            public Guid TemporaryId { get; set; }  // Gebruik een GUID voor de tijdelijke ID
            public Preferences Preferences { get; set; }
        }

        public class Preferences
        {
            public List<string> SelectedCategories { get; set; }
            public int RiskLevel { get; set; }  // 1-5 schaal
        }
    }
    public class Sessions
    {
        public class Session
        {
            public Guid SessionId { get; set; }
            public string HostName { get; set; }
            public List<Participant> Participants { get; set; }
            public SessionSettings Settings { get; set; }
            public string QrCode { get; set; }
        }

        public class Participant
        {
            public string UserName { get; set; }
            public Guid TemporaryId { get; set; }
        }

        public class SessionSettings
        {
            public List<string> SelectedCategories { get; set; }
            public int RiskLevel { get; set; }  // 1-5 schaal
            public List<Question> QuestionPool { get; set; }
        }

        public class Question
        {
            public string QuestionText { get; set; }
            public string Category { get; set; }
            public int RiskLevel { get; set; }  // 1-5 schaal
        }

    }
    public class GameDatas
    {
        public class GameData
        {
            public List<StandardQuestion> StandardQuestions { get; set; }
            public List<PersonalizedQuestion> PersonalizedQuestions { get; set; }
            public List<QuestionType> QuestionTypes { get; set; }
        }

        public class StandardQuestion
        {
            public string QuestionText { get; set; }
            public string Category { get; set; }
            public int RiskLevel { get; set; }  // 1-5 schaal
        }

        public class PersonalizedQuestion : StandardQuestion
        {
            public Guid CreatedBy { get; set; }  // UUID of user who created the question
        }

        public class QuestionType
        {
            public string TextQuestion { get; set; }
            public string PhotoQuestion { get; set; }
            public string Task { get; set; }
        }

    }
    
    public class TemporaryStorages
    {
        public class TemporaryStorage
        {
            public ActiveQuestion ActiveQuestion { get; set; }
            public GameStatus GameStatus { get; set; }
        }

        public class ActiveQuestion
        {
            public string QuestionText { get; set; }
            public string Category { get; set; }
            public int RiskLevel { get; set; }  // 1-5 schaal
        }

        public class GameStatus
        {
            public Guid CurrentPlayerId { get; set; }
            public List<string> TurnOrder { get; set; }
        }

    }
}
