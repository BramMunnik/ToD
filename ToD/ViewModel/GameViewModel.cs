using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ToD.Services;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Controls;

namespace ToD.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged
    {
        // --- Velden ---
        private readonly TruthOrDareService _service;
        private readonly Random _random;
        private List<string> _availablePlayers;
        private string _lastPlayer;
        private bool _challengeActive;
        private DateTime _startTime;

        private const int DrinkCountThreshold = 10; // Drempelwaarde voor het aantal drankkeuzes
        private const int ChallengeDuration = 3000; // Duur van de uitdaging in milliseconden

        // --- Properties ---
        private string _currentPlayer;
        private string _currentQuestion;
        private bool _requiresCamera;
        private bool _useApiQuestions;
        private int _gyroCount;

        public ObservableCollection<string> Members { get; set; } // Lijst van spelers
        public Dictionary<string, (int truthCount, int drinkCount)> PlayerChoices { get; set; } // Keuzes van de spelers (truth/drink)

        // De huidige speler
        public string CurrentPlayer
        {
            get => _currentPlayer;
            set => SetProperty(ref _currentPlayer, value);
        }

        // De huidige vraag
        public string CurrentQuestion
        {
            get => _currentQuestion;
            set => SetProperty(ref _currentQuestion, value);
        }

        // Of de vraag een camera vereist (voor foto of video)
        public bool RequiresCamera
        {
            get => _requiresCamera;
            set => SetProperty(ref _requiresCamera, value);
        }

        // Schakeloptie voor het ophalen van vragen via de API of uit een lokale lijst
        public bool UseApiQuestions
        {
            get => _useApiQuestions;
            set => SetProperty(ref _useApiQuestions, value);
        }

        // --- Commands ---
        public ICommand ChooseTruthCommand { get; }
        public ICommand ChooseDrinkCommand { get; }
        public ICommand EndGameCommand { get; }
        public ICommand UseCameraCommand { get; }

        // --- Constructor ---
        public GameViewModel(ObservableCollection<string> members, bool useApiQuestions)
        {
            // Initialiseer de ledenlijst en andere instellingen
            Members = members;
            _useApiQuestions = useApiQuestions;
            _service = new TruthOrDareService();
            _random = new Random();
            _availablePlayers = new List<string>(members);
            _lastPlayer = null;

            // Initialiseer de keuzes van de spelers
            PlayerChoices = Members.ToDictionary(member => member, member => (truthCount: 0, drinkCount: 0));

            // Koppel de commands aan de bijbehorende methodes
            ChooseTruthCommand = new Command(ChooseTruth);
            ChooseDrinkCommand = new Command(ChooseDrink);
            EndGameCommand = new Command(EndGame);
            UseCameraCommand = new Command(UseCamera);

            // Genereer een willekeurige speler en vraag
            GenerateRandomPlayerAndQuestion();
        }

        // --- Camera functie ---
        private async void UseCamera()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(); // Maak een foto met de camera
                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();
                    // Verwerk of sla de foto op zoals nodig
                    await App.Current.MainPage.DisplayAlert("Camera", "Foto succesvol genomen!", "Ok");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Fout", $"Kon geen foto nemen: {ex.Message}", "Ok");
            }

            // Genereer een nieuwe speler en vraag na het gebruik van de camera
            GenerateRandomPlayerAndQuestion();
        }

        // --- Willekeurige speler en vraag genereren ---
        public async void GenerateRandomPlayerAndQuestion()
        {
            // Als er geen beschikbare spelers meer zijn, reset de lijst
            if (_availablePlayers.Count == 0)
            {
                _availablePlayers = new List<string>(Members);
                _lastPlayer = null;
                GenerateRandomPlayerAndQuestion();
                return;
            }

            // Kies willekeurig een speler die nog niet de vorige speler is
            string chosenPlayer;
            do
            {
                chosenPlayer = _availablePlayers[_random.Next(_availablePlayers.Count)];
            } while (chosenPlayer == _lastPlayer);

            // Stel de geselecteerde speler in
            CurrentPlayer = chosenPlayer;
            _lastPlayer = chosenPlayer;
            _availablePlayers.Remove(CurrentPlayer);

            // Kies een vraag afhankelijk van of de API wordt gebruikt of niet
            CurrentQuestion = UseApiQuestions ? await _service.GetRandomQuestionAsync() : GetRandomQuestion();

            // Stel in of de vraag camera vereist
            RequiresCamera = CurrentQuestion.Contains("Neem een video") || CurrentQuestion.Contains("Maak een foto");
        }

        // --- Lijst met standaardvragen ---
        private string GetRandomQuestion()
        {
            var questions = new List<string>
            {
                "Wat is je grootste angst?", "Wat is je meest gênante moment?", "Wat is de grootste leugen die je ooit hebt verteld?",
                "Wie was je eerste crush?", "Wat is je meest gewaagde droom?", "Wat is iets wat bijna niemand over je weet?",
                "Heb je ooit iets gestolen, zelfs iets kleins?", "Wat is je grootste spijt?", "Wat is het meest bizarre wat je ooit hebt gegeten?",
                "Wie uit de groep vertrouw je het minst en waarom?", "Welke eigenschap van jezelf zou je willen veranderen?",
                "Heb je ooit een geheim van een vriend doorverteld?", "Wat is je vreemdste gewoonte?",
                "Wat is het ergste cadeau dat je ooit hebt gekregen?", "Als je onzichtbaar kon zijn voor een dag, wat zou je doen?",
                "Neem een video waarin je een lied zingt alsof je op een podium staat.", "Neem een video waarin je doet alsof je een beroemde acteur bent.",
                "Neem een video waarin je 10 seconden lang hardop lacht.", "Maak een foto van iets blauw in de kamer.",
                "Maak een foto van jezelf met je meest gekke gezicht.", "Maak een foto van je schoenen en plaats ze op een rare plek.",
                "Neem een video waarin je doet alsof je een kat bent.", "Neem een video waarin je drie grappige dansmoves uitvoert.",
                "Maak een foto van iets dat begint met de letter 'B'.", "Maak een foto van een willekeurig object en laat anderen raden wat het is.",
                "Neem een video waarin je een mop vertelt, maar blijf serieus kijken.", "Maak een foto van jezelf terwijl je een gek hoedje of item draagt.",
                "Neem een video waarin je een tongue-twister zegt, zoals: 'De kat krabt de krullen van de trap.'",
                "Maak een foto van iets in de kamer dat je nog nooit eerder hebt aangeraakt.", "Neem een video waarin je fluistert alsof je een geheim deelt."
            };

            // Kies een willekeurige vraag uit de lijst
            return questions[_random.Next(questions.Count)];
        }

        // --- Keuzes van de speler (Truth of Dare) ---
        public void ChooseTruth()
        {
            // Verhoog het aantal "truth" voor de huidige speler
            UpdatePlayerChoice((truthCount: 1, drinkCount: 0));
            // Genereer een nieuwe speler en vraag
            GenerateRandomPlayerAndQuestion();
        }

        // Werk de keuze van de speler bij
        private void UpdatePlayerChoice((int truthCount, int drinkCount) choice)
        {
            if (PlayerChoices.ContainsKey(CurrentPlayer))
            {
                var (truthCount, drinkCount) = PlayerChoices[CurrentPlayer];
                PlayerChoices[CurrentPlayer] = (truthCount + choice.truthCount, drinkCount + choice.drinkCount);
            }
        }

        // --- Drinken functie ---
        public void ChooseDrink()
        {
            // Verhoog het aantal "drink" voor de huidige speler
            if (PlayerChoices.ContainsKey(CurrentPlayer))
            {
                var (truthCount, drinkCount) = PlayerChoices[CurrentPlayer];
                drinkCount++;

                // Werk de keuze bij
                PlayerChoices[CurrentPlayer] = (truthCount, drinkCount);

                // Start de gyroscoop-uitdaging als het aantal drankkeuzes de drempel overschrijdt
                HandleGyroChallenge(drinkCount);
            }

            // Genereer een nieuwe speler en vraag
            GenerateRandomPlayerAndQuestion();
        }

        // --- Gyroscoop uitdaging ---
        private void HandleGyroChallenge(int drinkCount)
        {
            // Als het aantal drankkeuzes de drempel overschrijdt, start de uitdaging
            if (drinkCount >= DrinkCountThreshold)
            {
                _gyroCount = 0;
                ShowGyroChallengePopup();
            }
        }

        // Toon de gyroscoop-uitdaging popup
        private async void ShowGyroChallengePopup()
        {
            await Application.Current.MainPage.DisplayAlert("Gyro Challenge!", "Houd de telefoon 3 seconden stil!", "Start");
            StartGyroChallenge();
        }

        // Start de gyroscoop-uitdaging
        private void StartGyroChallenge()
        {
            if (Gyroscope.Default.IsSupported)
            {
                _challengeActive = true;
                _startTime = DateTime.Now;

                Gyroscope.Default.ReadingChanged += OnGyroscopeReadingChanged;
                Gyroscope.Default.Start(SensorSpeed.UI);
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Geen Gyroscoop", "Deze uitdaging werkt niet zonder gyroscoop!", "OK");
            }
        }

        // Verwerk de gyroscoopgegevens en controleer of de telefoon stil is
        private async void OnGyroscopeReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            if (!_challengeActive) return;

            var angularVelocity = e.Reading.AngularVelocity;
            bool isStable = Math.Abs(angularVelocity.X) < 0.05 && Math.Abs(angularVelocity.Y) < 0.05 && Math.Abs(angularVelocity.Z) < 0.05;

            // Controleer of de telefoon lang genoeg stil bleef
            if (isStable && (DateTime.Now - _startTime).TotalMilliseconds >= ChallengeDuration)
            {
                _challengeActive = false;
                Gyroscope.Default.ReadingChanged -= OnGyroscopeReadingChanged;
                Gyroscope.Default.Stop();

                await Application.Current.MainPage.DisplayAlert("Gefeliciteerd!", "Je hebt de telefoon stil gehouden!", "OK");
            }
            else if (!isStable && (DateTime.Now - _startTime).TotalMilliseconds >= ChallengeDuration)
            {
                _challengeActive = false;
                Gyroscope.Default.ReadingChanged -= OnGyroscopeReadingChanged;
                Gyroscope.Default.Stop();

                await Application.Current.MainPage.DisplayAlert("Uitdaging Mislukt", "Je kon de telefoon niet stil genoeg houden. Drink wat water.", "OK");
            }
            else if (!isStable)
            {
                // Reset de timer als de telefoon niet stil genoeg is
                _startTime = DateTime.Now;
            }
        }

        // --- Eindig het spel en toon de score ---
        public async void EndGame()
        {
            var scoreboard = string.Join("\n", PlayerChoices.Select(player => $"{player.Key} - Truth: {player.Value.truthCount}, Drink: {player.Value.drinkCount}"));
            await App.Current.MainPage.DisplayAlert("Scorebord", scoreboard, "Ok");
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }

        // --- Property Change Notification ---
        public event PropertyChangedEventHandler PropertyChanged;

        // Verzend een notificatie voor wijzigingen in de properties
        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        // Trigger de property change notificatie
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
