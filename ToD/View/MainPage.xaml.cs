using ToD.ViewModel;

namespace ToD
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void GoToSessionButton_Clicked(object sender, EventArgs e)
        {
            // Verwijder oude gebruikers voordat de nieuwe sessie wordt aangemaakt
            var viewModel = new SessionViewModel();
            await viewModel.ClearAllMembersAsync();

            // Navigeren naar de Session-pagina
            await Navigation.PushAsync(new Session());
        }


        private async void JoinSessionButton_Clicked(object sender, EventArgs e)
        {
            // Navigeren naar de join Session-pagina
            await Navigation.PushAsync(new JoinSession());
        }
    }
}