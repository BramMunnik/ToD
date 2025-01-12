using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ToD.Model;

namespace ToD.ViewModel
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        // DatabaseService wordt gebruikt voor interactie met de database
        private readonly DatabaseService _databaseService;

        // De sessiegegevens worden opgeslagen in dit model
        private SessionModel _session = new SessionModel();

        // Lijst van leden in de sessie
        private ObservableCollection<string> _members;
        public ObservableCollection<string> Members
        {
            get => _members;
            set
            {
                _members = value;
                OnPropertyChanged();
            }
        }

        // Lijst van geselecteerde categorieën
        private ObservableCollection<string> _selectedCategories = new ObservableCollection<string>();
        public ObservableCollection<string> SelectedCategories
        {
            get => _selectedCategories;
            set
            {
                _selectedCategories = value;
                OnPropertyChanged();
            }
        }

        // Eigenschap om in te stellen of er API-vragen gebruikt moeten worden
        private bool _useApiQuestions;
        public bool UseApiQuestions
        {
            get => _useApiQuestions;
            set
            {
                _useApiQuestions = value;
                OnPropertyChanged();
            }
        }

        // Command om een categorie te selecteren of te deselecteren
        public ICommand SelectCategoryCommand { get; }

        // Event om property veranderingen te melden aan de View
        public event PropertyChangedEventHandler PropertyChanged;

        // Constructor waarin de DatabaseService wordt geïnitialiseerd
        public SessionViewModel()
        {
            _databaseService = new DatabaseService();
            _members = new ObservableCollection<string>();
            SelectCategoryCommand = new Command<string>(SelectCategory);  // Koppelen van de SelectCategory-methode aan het commando
            LoadMembersAsync();  // Leden laden uit de database bij het opstarten
        }

        // Methode om een categorie toe te voegen of te verwijderen van de selectie
        public void SelectCategory(string category)
        {
            // Als de categorie al geselecteerd is, verwijder deze dan
            if (SelectedCategories.Contains(category))
            {
                SelectedCategories.Remove(category);
            }
            // Anders voeg de categorie toe aan de selectie
            else
            {
                SelectedCategories.Add(category);
            }
        }

        // Methode om de geselecteerde categorieën om te zetten naar een JSON-string
        public string GetSelectedCategoriesAsJson()
        {
            // Zet de lijst van geselecteerde categorieën om naar JSON
            return JsonConvert.SerializeObject(SelectedCategories);
        }

        // Methode om een nieuw lid toe te voegen
        public async void AddMember(string member)
        {
            if (!string.IsNullOrWhiteSpace(member) && !Members.Contains(member))
            {
                Members.Add(member);
                await SaveMemberToDatabaseAsync(member);  // Sla het lid op in de database
            }
        }

        // Methode om een lid te verwijderen
        public async void RemoveMember(string member)
        {
            if (Members.Contains(member))
            {
                Members.Remove(member);
                await DeleteMemberFromDatabaseAsync(member);  // Verwijder het lid uit de database
            }
        }

        // Methode om de naam van een lid te bewerken
        public async void EditMember(string oldName, string newName)
        {
            var index = Members.IndexOf(oldName);
            if (index != -1 && !string.IsNullOrWhiteSpace(newName))
            {
                // Controleer of de nieuwe naam al bestaat
                if (Members.Contains(newName))
                {
                    throw new InvalidOperationException("De nieuwe naam bestaat al in de lijst.");
                }

                // Werk de naam van het lid bij
                Members[index] = newName;
                await UpdateMemberInDatabaseAsync(oldName, newName);  // Werk het lid in de database bij
            }
        }

        // Methode om leden uit de database te laden
        private async Task LoadMembersAsync()
        {
            var users = await _databaseService.GetItemsAsync<User>();  // Haal de lijst van gebruikers op uit de database
            foreach (var user in users)
            {
                Members.Add(user.Name);  // Voeg de gebruikersnamen toe aan de ledenlijst
            }
        }

        // Methode om een nieuw lid op te slaan in de database
        private async Task SaveMemberToDatabaseAsync(string member)
        {
            var user = new User { Name = member, TemporaryId = Guid.NewGuid() };  // Maak een nieuw User-object aan
            await _databaseService.SaveItemAsync(user);  // Sla het lid op in de database
        }

        // Methode om een lid uit de database te verwijderen
        private async Task DeleteMemberFromDatabaseAsync(string member)
        {
            var user = (await _databaseService.GetItemsAsync<User>())
                .FirstOrDefault(u => u.Name == member);  // Zoek het lid in de database
            if (user != null)
            {
                await _databaseService.DeleteItemAsync(user);  // Verwijder het lid uit de database
            }
        }

        // Methode om de gegevens van een lid in de database bij te werken
        private async Task UpdateMemberInDatabaseAsync(string oldName, string newName)
        {
            var user = (await _databaseService.GetItemsAsync<User>())
                .FirstOrDefault(u => u.Name == oldName);  // Zoek het oude lid in de database
            if (user != null)
            {
                user.Name = newName;  // Werk de naam bij
                await _databaseService.SaveItemAsync(user);  // Sla de bijgewerkte gegevens op in de database
            }
        }

        // Methode om de sessie op te slaan in de database
        public async Task SaveSessionToDatabaseAsync(SessionModel session)
        {
            await _databaseService.SaveItemAsync(session);  // Sla de sessie op in de database
        }

        // Methode om alle leden uit de database te verwijderen
        public async Task ClearAllMembersAsync()
        {
            var users = await _databaseService.GetItemsAsync<User>();  // Haal alle gebruikers uit de database
            foreach (var user in users)
            {
                await _databaseService.DeleteItemAsync(user);  // Verwijder elk lid uit de database
            }

            Members.Clear();  // Maak ook de lokale ledenlijst leeg
        }

        // Methode om PropertyChanged event aan te roepen wanneer een property verandert
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}