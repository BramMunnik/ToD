namespace ToD;

public partial class JoinSession : ContentPage
{
	public JoinSession()
	{
		InitializeComponent();
	}
    private async void JoinName_Clicked(object sender, EventArgs e)
    {
        // Navigeren naar de join Session-pagina
        await Navigation.PushAsync(new JoinEnterName());
    }
}