using System;

namespace ToD
{
    public partial class QuestionsPage : ContentPage
    {
        public QuestionsPage()
        {
            InitializeComponent();
        }

        // Event: Star rating clicked
        private void StarClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton clickedStar)
            {
                string starName = clickedStar.StyleId; // Get the StyleId (e.g., "1", "2", ...)
                int starIndex = int.Parse(starName);

                // Update the stars based on the clicked index
                UpdateStars(starIndex);
            }
        }

        private void UpdateStars(int selectedStarIndex)
        {
            for (int i = 1; i <= 5; i++)
            {
                var star = (ImageButton)this.FindByName($"Star{i}");
                star.Source = i <= selectedStarIndex ? "star_filled.png" : "star_empty.png";
            }
        }

        // Event: Recommended Questions button clicked
        private async void RecommendedQuestionsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Recommended Questions", "Hier kun je aanbevolen vragen selecteren.", "OK");
        }

        // Event: Custom Questions button clicked
        private async void CustomQuestionsClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Custom Questions", "Hier kun je je eigen vragen toevoegen.", "OK");
        }

        // Event: Save button clicked
        private async void SaveClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Saved", "Je voorkeuren zijn opgeslagen!", "OK");
        }
    }
}
