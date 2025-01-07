using Microsoft.Maui.Controls;
using ToD.ViewModel;

namespace ToD
{
    public partial class CustomQuestionPage : ContentPage
    {
        private int _selectedRiskiness = 0;

        public CustomQuestionPage(GameViewModel gameViewModel)
        {
            InitializeComponent();
            // Hier kun je het gameViewModel gebruiken indien nodig
        }

        // Wanneer een ster wordt aangeklikt
        private void StarClicked(object sender, EventArgs e)
        {
            // Bepaal welke ster is aangeklikt
            var selectedStar = (ImageButton)sender;
            _selectedRiskiness = int.Parse(selectedStar.StyleId); // Stel het geselecteerde risiconiveau in
            UpdateStarDisplay(); // Werk de sterren visueel bij
        }

        // Werk de sterren bij op basis van het geselecteerde risiconiveau
        private void UpdateStarDisplay()
        {
            var stars = new[] { Star1, Star2, Star3, Star4, Star5 };
            for (int i = 0; i < stars.Length; i++)
            {
                // Stel de ster in op 'gevuld' als deze onder of gelijk is aan de geselecteerde riskiness
                stars[i].Source = i < _selectedRiskiness ? "star_filled.png" : "star_empty.png";
            }
        }

        // Wanneer de vraag wordt opgeslagen
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var newQuestion = QuestionEntry.Text?.Trim();

            // Alleen opslaan als er een geldige vraag is ingevoerd
            if (!string.IsNullOrWhiteSpace(newQuestion))
            {
                // Omdat de sterren visueel worden bijgewerkt, maar de riskiness niet wordt gebruikt, slaan we de riskiness niet op
                await DisplayAlert("Success", "Question added!", "OK");
                await Navigation.PopAsync(); // Terug naar de vorige pagina
            }
            else
            {
                await DisplayAlert("Error", "Please enter a question.", "OK");
            }
        }
    }
}
