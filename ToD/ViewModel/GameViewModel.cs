using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ToD.Services;
using System.Collections.Generic;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;


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
    // Algemene vragen
    "Wat is je grootste angst?",
    "Wat is je meest gênante moment?",
    "Wat is de grootste leugen die je ooit hebt verteld?",
    "Wie was je eerste crush?",
    "Wat is je meest gewaagde droom?",
    "Wat is iets wat bijna niemand over je weet?",
    "Heb je ooit iets gestolen, zelfs iets kleins?",
    "Wat is je grootste spijt?",
    "Wat is het meest bizarre wat je ooit hebt gegeten?",
    "Wie uit de groep vertrouw je het minst en waarom?",
    "Welke eigenschap van jezelf zou je willen veranderen?",
    "Heb je ooit een geheim van een vriend doorverteld?",
    "Wat is je vreemdste gewoonte?",
    "Wat is het ergste cadeau dat je ooit hebt gekregen?",
    "Als je onzichtbaar kon zijn voor een dag, wat zou je doen?",

    // Camera-opdrachten
    "Neem een video waarin je een lied zingt alsof je op een podium staat.",
    "Neem een video waarin je doet alsof je een beroemde acteur bent.",
    "Neem een video waarin je 10 seconden lang hardop lacht.",
    "Maak een foto van iets blauw in de kamer.",
    "Maak een foto van jezelf met je meest gekke gezicht.",
    "Maak een foto van je schoenen en plaats ze op een rare plek.",
    "Neem een video waarin je doet alsof je een kat bent.",
    "Neem een video waarin je drie grappige dansmoves uitvoert.",
    "Maak een foto van iets dat begint met de letter 'B'.",
    "Maak een foto van een willekeurig object en laat anderen raden wat het is.",
    "Neem een video waarin je een mop vertelt, maar blijf serieus kijken.",
    "Maak een foto van jezelf terwijl je een gek hoedje of item draagt.",
    "Neem een video waarin je een tongue-twister zegt, zoals: 'De kat krabt de krullen van de trap.'",
    "Maak een foto van iets in de kamer dat je nog nooit eerder hebt aangeraakt.",
    "Neem een video waarin je fluistert alsof je een geheim deelt."
};

        private bool _requiresCamera;
        public bool RequiresCamera
        {
            get => _requiresCamera;
            set
            {
                _requiresCamera = value;
                OnPropertyChanged();
            }
        }



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
        public ICommand UseCameraCommand { get; }

        public GameViewModel(ObservableCollection<string> members, bool useApiQuestions)
        {
            UseCameraCommand = new Command(UseCamera);

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
        private async void UseCamera()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    // Verwerk of sla de foto op zoals nodig
                    await App.Current.MainPage.DisplayAlert("Camera", "Foto is succesvol genomen!", "Ok");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Fout", $"Kon geen foto nemen: {ex.Message}", "Ok");
            }

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
                var randomQuestion = _questions[_random.Next(_questions.Count)];
                CurrentQuestion = randomQuestion;

                // Stel RequiresCamera in
                RequiresCamera = randomQuestion.Contains("Neem een video") || randomQuestion.Contains("Maak een foto");
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
