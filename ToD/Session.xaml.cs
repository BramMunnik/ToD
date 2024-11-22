namespace ToD
{
    public partial class Session : ContentPage
    {
        public Session()
        {
            InitializeComponent();
        }

        private void Questions_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestionsPage());
        }
    }
}