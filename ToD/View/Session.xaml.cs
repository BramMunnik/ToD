using Microsoft.Maui.Controls;
using ToD.ViewModel;

namespace ToD
{
    public partial class Session : ContentPage
    {
        public SessionViewModel ViewModel { get; set; }

        public Session()
        {
            InitializeComponent();
            ViewModel = new SessionViewModel();
            BindingContext = ViewModel;
        }

        // Methode om een nieuw lid toe te voegen
        private void OnAddMemberClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MemberEntry.Text))
            {
                ViewModel.AddMember(MemberEntry.Text);
                MemberEntry.Text = string.Empty;
            }
        }

        // Methode om een lid te verwijderen
        private void OnRemoveMemberClicked(object sender, EventArgs e)
        {
            var member = (string)((Button)sender).CommandParameter;
            ViewModel.RemoveMember(member);
        }

        // Methode om een lid te bewerken
        private async void OnEditMemberClicked(object sender, EventArgs e)
        {
            try
            {
                var member = (string)((Button)sender).CommandParameter;
                var newName = await DisplayPromptAsync("Bewerk naam", "Voer nieuwe naam in:", initialValue: member);

                if (!string.IsNullOrWhiteSpace(newName) && newName != member)
                {
                    // Controleer of de naam al bestaat
                    if (ViewModel.Members.Contains(newName))
                    {
                        await DisplayAlert("Fout", "De naam bestaat al. Kies een andere naam.", "OK");
                        return;
                    }

                    ViewModel.EditMember(member, newName);
                }
            }
            catch (Exception ex)
            {
                // Log de fout en toon een melding
                Console.WriteLine($"Fout bij het bewerken van de naam: {ex.Message}");
                await DisplayAlert("Fout", "Er is een fout opgetreden bij het bewerken van de naam.", "OK");
            }
        }

        // Methode om naar de QuestionsPage te navigeren
        private void Questions_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestionsPage());
        }

        private void Start_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GamePage(ViewModel.Members));
        }
    }
}