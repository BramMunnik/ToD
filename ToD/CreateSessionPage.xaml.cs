namespace ToD
{
    public partial class CreateSessionPage : ContentPage
    {
        public CreateSessionPage()
        {
            InitializeComponent();
            BindingContext = new CreateSessionViewModel();
        }
    }
}
