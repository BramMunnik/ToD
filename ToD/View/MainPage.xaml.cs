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
            var viewModel = new SessionViewModel();
            await viewModel.ClearAllMembersAsync();

            await Navigation.PushAsync(new Session());
        }

        private async void JoinSessionButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new JoinSession());
        }
    }
}