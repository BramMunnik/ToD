namespace ToD;

public partial class JoinSession : ContentPage
{
	public JoinSession()
	{
		InitializeComponent();
	}
    private async void JoinName_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new JoinEnterName());
    }
}