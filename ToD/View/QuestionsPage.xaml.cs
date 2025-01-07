namespace ToD
{
    public partial class QuestionsPage : ContentPage
    {
        public QuestionsPage()
        {
            InitializeComponent();
        }
        private void StarClicked(object sender, EventArgs e)
        {
            // Controleer op welk sterretje is geklikt
            if (sender is ImageButton clickedStar)
            {
                // Haal de sterindex op (bijv. Star1, Star2, ...)
                string starName = clickedStar.StyleId; 
                int starIndex = int.Parse(starName.Replace("Star", ""));

                // Verander de sterren
                UpdateStars(starIndex);
            }
        }

        private void UpdateStars(int selectedStarIndex)
        {
            // Loop door alle sterren en pas ze aan
            for (int i = 1; i <= 5; i++)
            {
                var star = (ImageButton)this.FindByName($"Star{i}");
                if (i <= selectedStarIndex)
                {
                    star.Source = "star_filled.png"; // Gevulde ster
                }
                else
                {
                    star.Source = "star_empty.png"; // Lege ster
                }
            }
        }
    }

}