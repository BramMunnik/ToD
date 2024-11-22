namespace ToD
{
    public partial class NewPage : ContentPage
    {
        public NewPage()
        {
            InitializeComponent();
        }

        private async void GoToSessionButton_Clicked(object sender, EventArgs e)
        {
            // Navigeren naar de Session-pagina
            await Navigation.PushAsync(new Session());
        }
    }
}
