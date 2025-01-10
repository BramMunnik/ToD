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

        // Bijhouden hoeveel keer elke speler 'truth' en 'drink' kiest
        public Dictionary<string, (int truthCount, int drinkCount)> PlayerChoices { get; set; }

        public ObservableCollection<string> Members { get; set; }

        private List<string> availablePlayers; // Lijst met beschikbare spelers
        private string lastPlayer; // Bijhouden van de vorige speler

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

        public GameViewModel(ObservableCollection<string> members)
        {
            Members = members;
            _service = new TruthOrDareService();
            _random = new Random();
            PlayerChoices = new Dictionary<string, (int truthCount, int drinkCount)>();
            availablePlayers = new List<string>(Members); // Zet de originele lijst in de beschikbare lijst
            lastPlayer = null; // Initialiseer de vorige speler

            ChooseTruthCommand = new Command(ChooseTruth);
            ChooseDrinkCommand = new Command(ChooseDrink);
            EndGameCommand = new Command(EndGame);

            GenerateRandomPlayerAndQuestion();
        }

        public async void GenerateRandomPlayerAndQuestion()
        {
            if (availablePlayers.Count > 0)
            {
                // Kies een willekeurige speler uit de beschikbare spelerslijst, maar zorg ervoor dat de vorige speler niet opnieuw wordt gekozen
                string chosenPlayer = availablePlayers[_random.Next(availablePlayers.Count)];

                // Als de gekozen speler dezelfde is als de vorige speler, kies dan opnieuw
                while (chosenPlayer == lastPlayer)
                {
                    chosenPlayer = availablePlayers[_random.Next(availablePlayers.Count)];
                }

                CurrentPlayer = chosenPlayer;
                lastPlayer = chosenPlayer; // Zet de vorige speler

                availablePlayers.Remove(CurrentPlayer); // Verwijder deze speler van de lijst
            }
            else
            {
                // Als alle spelers aan de beurt zijn geweest, reset de lijst
                availablePlayers = new List<string>(Members);
                lastPlayer = null; // Reset de vorige speler
                GenerateRandomPlayerAndQuestion(); // Kies opnieuw een speler
            }

            CurrentQuestion = await _service.GetRandomQuestionAsync();
        }

        public void ChooseTruth()
        {
            if (PlayerChoices.ContainsKey(CurrentPlayer))
            {
                var currentChoice = PlayerChoices[CurrentPlayer];
                PlayerChoices[CurrentPlayer] = (currentChoice.truthCount + 1, currentChoice.drinkCount);
            }
            else
            {
                PlayerChoices[CurrentPlayer] = (1, 0); // Eerste keuze is 'truth'
            }

            GenerateRandomPlayerAndQuestion(); // Genereer nieuwe speler en vraag
        }

        public void ChooseDrink()
        {
            if (PlayerChoices.ContainsKey(CurrentPlayer))
            {
                var currentChoice = PlayerChoices[CurrentPlayer];
                PlayerChoices[CurrentPlayer] = (currentChoice.truthCount, currentChoice.drinkCount + 1);
            }
            else
            {
                PlayerChoices[CurrentPlayer] = (0, 1); // Eerste keuze is 'drink'
            }

            GenerateRandomPlayerAndQuestion(); // Genereer nieuwe speler en vraag
        }

        public async void EndGame()
        {
            // Genereer de tekst voor het scoreboard
            var scoreboard = "";
            foreach (var player in PlayerChoices)
            {
                scoreboard += $"{player.Key} - Truth: {player.Value.truthCount}, Drink: {player.Value.drinkCount}\n";
            }

            // Toon het scoreboard als een pop-up
            await App.Current.MainPage.DisplayAlert("Scoreboard", scoreboard, "Ok");

            // Navigeer naar de MainPage na het sluiten van de pop-up
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
