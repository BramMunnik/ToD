using System;
using ToD.ViewModel;
using ToD.Model;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ToD
{
    public partial class Session : ContentPage
    {
        private SessionViewModel ViewModel => BindingContext as SessionViewModel;

        public Session()
        {
            InitializeComponent();
            BindingContext = new SessionViewModel();
        }

        // Toevoegen van een lid aan de lijst
        private void OnAddMemberClicked(object sender, EventArgs e)
        {
            string memberName = MemberEntry.Text;

            if (!string.IsNullOrWhiteSpace(memberName))
            {
                ViewModel.AddMember(memberName);
                MemberEntry.Text = string.Empty; // Leeg het invoerveld
            }
        }

        // Bewerken van een bestaande lidnaam
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

        // Verwijderen van een lid uit de lijst
        private async void OnRemoveMemberClicked(object sender, EventArgs e)
        {
            string memberName = (string)((Button)sender).CommandParameter;

            bool confirm = await DisplayAlert("Bevestig", $"Weet je zeker dat je {memberName} wilt verwijderen?", "Ja", "Nee");

            if (confirm)
            {
                ViewModel.RemoveMember(memberName);
            }
        }

        // Start de sessie en navigeer naar de volgende pagina
        private async void Start_Clicked(object sender, EventArgs e)
        {
            var viewModel = (SessionViewModel)BindingContext;

            bool useApiQuestions = viewModel.UseApiQuestions;
            var members = viewModel.Members;

            if (members.Count == 0)
            {
                await DisplayAlert("Fout", "Voeg ten minste één lid toe om het spel te starten.", "OK");
                return;
            }

            var session = new SessionModel
            {
                SessionID = Guid.NewGuid(),
                HostID = Guid.NewGuid(), // Hier kun je de daadwerkelijke host-ID invullen
            };

            await ViewModel.SaveSessionToDatabaseAsync(session);
            Console.WriteLine("Sessiemodel succesvol opgeslagen.");

            await Navigation.PushAsync(new GamePage(members, useApiQuestions));
        }
    }
}