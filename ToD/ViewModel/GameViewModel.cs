using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ToD.Services;
using System.Collections.Generic;

namespace ToD.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {

        private readonly TruthOrDareService _service;
        private readonly Random _random;
        private string _currentPlayer;
        private string _currentQuestion;
        private readonly List<string> _questions = new()
        {
            "What's your biggest fear?",
            "What is your most embarrassing moment?",
            "Who was your first crush?",
            "What is your biggest regret?",
            "What's your weirdest habit?"
        };

        private List<string> availablePlayers;
        private string lastPlayer;

        private bool _useApiQuestions; // Schakeloptie
        public bool UseApiQuestions
        {
            get => _useApiQuestions;
            set
            {
                _useApiQuestions = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Members { get; set; }
        public Dictionary<string, (int truthCount, int drinkCount)> PlayerChoices { get; set; }

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

        public ICommand ChooseTruthCommand { get; }
        public ICommand ChooseDrinkCommand { get; }
        public ICommand EndGameCommand { get; }

        public GameViewModel(ObservableCollection<string> members, bool useApiQuestions)
        {
            Members = members;
            _useApiQuestions = useApiQuestions;
            _service = new TruthOrDareService();
            _random = new Random();
            availablePlayers = new List<string>(Members);
            lastPlayer = null;
            UseApiQuestions = useApiQuestions; // Set the value for UseApiQuestions

            // Initialiseer PlayerChoices
            PlayerChoices = new Dictionary<string, (int truthCount, int drinkCount)>();
            foreach (var member in Members)
            {
                PlayerChoices[member] = (0, 0);
            }

            ChooseTruthCommand = new Command(ChooseTruth);
            ChooseDrinkCommand = new Command(ChooseDrink);
            EndGameCommand = new Command(EndGame);

            GenerateRandomPlayerAndQuestion();
        }

        public async void GenerateRandomPlayerAndQuestion()
        {
            if (availablePlayers.Count > 0)
            {
                string chosenPlayer = availablePlayers[_random.Next(availablePlayers.Count)];
                while (chosenPlayer == lastPlayer)
                {
                    chosenPlayer = availablePlayers[_random.Next(availablePlayers.Count)];
                }

                CurrentPlayer = chosenPlayer;
                lastPlayer = chosenPlayer;

                availablePlayers.Remove(CurrentPlayer);
            }
            else
            {
                availablePlayers = new List<string>(Members);
                lastPlayer = null;
                GenerateRandomPlayerAndQuestion();
                return;
            }

            if (UseApiQuestions)
            {
                CurrentQuestion = await _service.GetRandomQuestionAsync();
            }
            else
            {
                CurrentQuestion = _questions[_random.Next(_questions.Count)];
            }
        }

        public void ChooseTruth()
        {
            if (PlayerChoices.ContainsKey(CurrentPlayer))
            {
                var (truthCount, drinkCount) = PlayerChoices[CurrentPlayer];
                PlayerChoices[CurrentPlayer] = (truthCount + 1, drinkCount);
            }
            GenerateRandomPlayerAndQuestion();
        }

        public void ChooseDrink()
        {
            if (PlayerChoices.ContainsKey(CurrentPlayer))
            {
                var (truthCount, drinkCount) = PlayerChoices[CurrentPlayer];
                PlayerChoices[CurrentPlayer] = (truthCount, drinkCount + 1);
            }
            GenerateRandomPlayerAndQuestion();
        }

        public async void EndGame()
        {
            var scoreboard = "";
            foreach (var player in PlayerChoices)
            {
                scoreboard += $"{player.Key} - Truth: {player.Value.truthCount}, Drink: {player.Value.drinkCount}\n";
            }

            await App.Current.MainPage.DisplayAlert("Scoreboard", scoreboard, "Ok");
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
