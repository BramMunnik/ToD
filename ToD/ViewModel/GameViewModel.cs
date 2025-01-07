using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToD.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly List<string> _questions = new List<string>
        {
            "What's your biggest fear?",
            "What is your most embarrassing moment?",
            "Who was your first crush?",
            "What is your biggest regret?",
            "What's your weirdest habit?"
        };

        private Random _random = new Random();
        private string _currentPlayer;
        private string _currentQuestion;

        public ObservableCollection<string> Members { get; set; }

        public string CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                OnPropertyChanged();
            }
        }

        public string CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        public GameViewModel(ObservableCollection<string> members)
        {
            Members = members;
            GenerateRandomPlayerAndQuestion();
        }

        public void GenerateRandomPlayerAndQuestion()
        {
            if (Members.Count > 0)
            {
                CurrentPlayer = Members[_random.Next(Members.Count)];
            }
            CurrentQuestion = _questions[_random.Next(_questions.Count)];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
