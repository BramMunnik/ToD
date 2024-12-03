namespace ToD;

public partial class JoinEnterName : ContentPage
{
	public JoinEnterName()
	{
		InitializeComponent();
	}
    private async void OnJoinClicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text;

        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Error", "Please enter your name before joining.", "OK");
            return;
        }

        await Navigation.PushAsync(new Wait());
    }
}