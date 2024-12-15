using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace ToD
{
    public partial class Session : ContentPage
    {
        // ObservableCollection houdt de leden bij
        private ObservableCollection<string> _members;

        public Session()
        {
            InitializeComponent();

            // Initialiseer de lijst met leden
            _members = new ObservableCollection<string>
            {
            };

            // Koppel de lijst aan de ListView
            MembersListView.ItemsSource = _members;
        }

        // Methode om een nieuw lid toe te voegen
        private void OnAddMemberClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MemberEntry.Text))
            {
                // Voeg nieuw lid toe aan de lijst
                _members.Add(MemberEntry.Text);

                // Reset het invoerveld
                MemberEntry.Text = string.Empty;
            }
        }

        // Methode om een lid te verwijderen
        private void OnRemoveMemberClicked(object sender, EventArgs e)
        {
            var member = (string)((Button)sender).CommandParameter;
            if (_members.Contains(member))
            {
                _members.Remove(member);
            }
        }

        // Methode om een lid te bewerken (naam aanpassen)
        private async void OnEditMemberClicked(object sender, EventArgs e)
        {
            var member = (string)((Button)sender).CommandParameter;
            var newName = await DisplayPromptAsync("Bewerk naam", "Voer nieuwe naam in:", initialValue: member);
            if (!string.IsNullOrWhiteSpace(newName))
            {
                var index = _members.IndexOf(member);
                if (index != -1)
                {
                    _members[index] = newName;
                }
            }
        }

        // Methode om naar de QuestionsPage te navigeren
        private void Questions_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new QuestionsPage());
        }
    }
}
