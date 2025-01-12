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
            await DisplayAlert("Fout", "Voer een naam in om te spelen", "OK");
            return;
        }

        await Navigation.PushAsync(new Wait());
    }
}