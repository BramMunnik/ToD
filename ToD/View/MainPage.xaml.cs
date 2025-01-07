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