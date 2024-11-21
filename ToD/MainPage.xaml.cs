namespace ToD
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void LogInButton_Clicked(object sender, EventArgs e)
        {
            bool isUsernameEmpty = string.IsNullOrEmpty(UsernameEntry.Text);
            bool isPasswordEmpty = string.IsNullOrEmpty(PasswordEntry.Text);
        
            if (isUsernameEmpty)
            {
                UsernameEntry.Placeholder = "vul iets in";
            } else if (isPasswordEmpty)
            {
                PasswordEntry.Placeholder = "vul iets in";
            }
            else
            {
                Navigation.PushAsync(new NewPage());
            }

        }
    }
}