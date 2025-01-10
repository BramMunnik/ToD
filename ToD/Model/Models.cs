using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToD.Model
{
    public class User
    {
        [Key]
        public Guid UserID { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation property: one user can host many sessions
        public ICollection<Session> Sessions { get; set; }
    }

    public class Session
    {
        [Key]
        public Guid SessionID { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public Guid HostID { get; set; }

        public User Host { get; set; }

        [Required]
        public string SelectedCategories { get; set; } // JSON string of categories

        [Range(1, 5)]
        public int DaringLevel { get; set; }

        [Required]
        public string QuestionPool { get; set; } // JSON string of QuestionIDs

        public string QRCode { get; set; }

        // Navigation property
        public ICollection<Participant> Participants { get; set; }
    }

    public class Participant
    {
        [Key]
        public Guid ParticipantID { get; set; }

        [Required]
        [ForeignKey(nameof(Session))]
        public Guid SessionID { get; set; }

        public Session Session { get; set; }

        [ForeignKey(nameof(User))]
        public Guid? UserID { get; set; } // Nullable for anonymous users

        public User User { get; set; }

        [Required]
        public Guid TemporaryID { get; set; }
    }

    public class GameData
    {
        [Key]
        public Guid QuestionID { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public string Category { get; set; }

        [Range(1, 5)]
        public int DaringLevel { get; set; }

        [ForeignKey(nameof(User))]
        public Guid? CreatedBy { get; set; }

        public User Creator { get; set; }

        [Required]
        public string QuestionType { get; set; }

        public string PhotoLink { get; set; }
    }

    public class TemporaryStorage
    {
        [Key]
        [ForeignKey(nameof(Session))]
        public Guid SessionID { get; set; }

        public Session Session { get; set; }

        [ForeignKey(nameof(GameData))]
        public Guid? ActiveQuestionID { get; set; }

        public GameData ActiveQuestion { get; set; }

        [ForeignKey(nameof(User))]
        public Guid? CurrentPlayerID { get; set; }

        public User CurrentPlayer { get; set; }

        [Required]
        public string TurnOrder { get; set; } // JSON array of UserIDs or TemporaryIDs
    }
}