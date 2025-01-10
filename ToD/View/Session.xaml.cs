using System;
using ToD.ViewModel;
using ToD.Model;

namespace ToD
{
    public partial class Session : ContentPage
    {
        private SessionViewModel ViewModel => BindingContext as SessionViewModel;

        public Session()
        {
            InitializeComponent();
        }

        private void OnAddMemberClicked(object sender, EventArgs e)
        {
            string memberName = MemberEntry.Text;

            if (!string.IsNullOrWhiteSpace(memberName))
            {
                ViewModel.AddMember(memberName);
                MemberEntry.Text = string.Empty; // Clear the input field
            }
        }

        private async void OnEditMemberClicked(object sender, EventArgs e)
        {
            string oldName = (string)((Button)sender).CommandParameter;

            string newName = await DisplayPromptAsync("Bewerk Lid", $"Wijzig de naam van {oldName}:", initialValue: oldName);

            if (!string.IsNullOrWhiteSpace(newName))
            {
                try
                {
                    ViewModel.EditMember(oldName, newName);
                }
                catch (InvalidOperationException ex)
                {
                    await DisplayAlert("Fout", ex.Message, "OK");
                }
            }
        }

        private async void OnRemoveMemberClicked(object sender, EventArgs e)
        {
            string memberName = (string)((Button)sender).CommandParameter;

            bool confirm = await DisplayAlert("Bevestig", $"Weet je zeker dat je {memberName} wilt verwijderen?", "Ja", "Nee");

            if (confirm)
            {
                ViewModel.RemoveMember(memberName);
            }
        }

        private async void Questions_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QuestionsPage());
        }

        private async void Start_Clicked(object sender, EventArgs e)
        {
            var members = ViewModel.Members; // Dit zijn de leden die in de ViewModel staan
            if (members.Count == 0)
            {
                await DisplayAlert("Fout", "Voeg ten minste één lid toe om het spel te starten.", "OK");
                return;
            }

            // Maak een nieuwe sessie aan en sla deze op in de database
            var session = new SessionModel
            {
                SessionID = Guid.NewGuid(), // Genereer een nieuwe unieke sessie-ID
                HostID = Guid.NewGuid(),    // Dit zou de ID van de host moeten zijn, afhankelijk van je logica
                SelectedCategories = "[]",  // Voeg de gekozen categorieën toe als JSON string
                DaringLevel = 3,           // Stel het daring level in
                QuestionPool = "[]",       // Voeg een lege vraagpool toe, of vul deze in
                QRCode = "path/to/qr/code" // Voeg de QR-code toe, indien nodig
            };

            // Sla de sessie op in de database
            await ViewModel.SaveSessionToDatabaseAsync(session);
            Console.WriteLine("Sessiemodel succesvol opgeslagen.");

            // Navigeer naar de volgende pagina (bijv. GamePage)
            await Navigation.PushAsync(new GamePage(members));
        }


    }
}
