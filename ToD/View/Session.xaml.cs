using System;
using ToD.ViewModel;

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
            var members = ViewModel.Members; // Assuming ViewModel.Members is an ObservableCollection<string>
            if (members.Count == 0)
            {
                await DisplayAlert("Fout", "Voeg ten minste één lid toe om het spel te starten.", "OK");
                return;
            }

            await Navigation.PushAsync(new GamePage(members));
        }

    }
}
