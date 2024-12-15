using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace ToD
{
    public partial class GamePage : ContentPage
    {
        private GameViewModel _viewModel;

        public GamePage(ObservableCollection<string> members)
        {
            InitializeComponent();
            _viewModel = new GameViewModel(members);
            BindingContext = _viewModel;
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            _viewModel.GenerateRandomPlayerAndQuestion();
        }

        private async void OnEndGameClicked(object sender, EventArgs e)
        {
            bool endGame = await DisplayAlert("End Game", "Weet je zeker dat je het spel wilt beëindigen?", "Ja", "Nee");
            if (endGame)
            {
                await Navigation.PopToRootAsync();
            }
        }
    }
}
