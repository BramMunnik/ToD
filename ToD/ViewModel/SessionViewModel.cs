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

        private SessionModel _session = new SessionModel();

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

        private readonly DatabaseService _databaseService;

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

        public ICommand SelectCategoryCommand { get; }

        public SessionViewModel()
        {
            _databaseService = new DatabaseService();
            _members = new ObservableCollection<string>();
            SelectCategoryCommand = new Command<string>(SelectCategory);  // Koppelen van de Command
            LoadMembersAsync();
        }

        // Toevoegen van categorieën aan de selectie
        public void SelectCategory(string category)
        {
            if (SelectedCategories.Contains(category))
            {
                SelectedCategories.Remove(category);  // Verwijder de categorie als deze al is geselecteerd
            }
            else
            {
                SelectedCategories.Add(category);  // Voeg de categorie toe als deze nog niet is geselecteerd
            }
        }

        // Methode om de geselecteerde categorieën op te slaan als JSON-string
        public string GetSelectedCategoriesAsJson()
        {
            return JsonConvert.SerializeObject(SelectedCategories);  // Zet de lijst van geselecteerde categorieën om in een JSON-string
        }

        // Toevoegen, verwijderen en bewerken van leden
        public async void AddMember(string member)
        {
            if (!string.IsNullOrWhiteSpace(member) && !Members.Contains(member))
            {
                Members.Add(member);
                await SaveMemberToDatabaseAsync(member);
            }
        }

        public async void RemoveMember(string member)
        {
            if (Members.Contains(member))
            {
                Members.Remove(member);
                await DeleteMemberFromDatabaseAsync(member);
            }
        }

        public async void EditMember(string oldName, string newName)
        {
            var index = Members.IndexOf(oldName);
            if (index != -1 && !string.IsNullOrWhiteSpace(newName))
            {
                if (Members.Contains(newName))
                {
                    throw new InvalidOperationException("De nieuwe naam bestaat al in de lijst.");
                }

                Members[index] = newName;
                await UpdateMemberInDatabaseAsync(oldName, newName);
            }
        }

        private async Task LoadMembersAsync()
        {
            var users = await _databaseService.GetItemsAsync<User>();
            foreach (var user in users)
            {
                Members.Add(user.Name);
            }
        }

        private async Task SaveMemberToDatabaseAsync(string member)
        {
            var user = new User { Name = member, TemporaryId = Guid.NewGuid() };
            await _databaseService.SaveItemAsync(user);
        }

        private async Task DeleteMemberFromDatabaseAsync(string member)
        {
            var user = (await _databaseService.GetItemsAsync<User>())
                .FirstOrDefault(u => u.Name == member);
            if (user != null)
            {
                await _databaseService.DeleteItemAsync(user);
            }
        }

        private async Task UpdateMemberInDatabaseAsync(string oldName, string newName)
        {
            var user = (await _databaseService.GetItemsAsync<User>())
                .FirstOrDefault(u => u.Name == oldName);
            if (user != null)
            {
                user.Name = newName;
                await _databaseService.SaveItemAsync(user);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task SaveSessionToDatabaseAsync(SessionModel session)
        {
            // Sla de sessie op in de database
            await _databaseService.SaveItemAsync(session);
        }

        public async Task ClearAllMembersAsync()
        {
            var users = await _databaseService.GetItemsAsync<User>();
            foreach (var user in users)
            {
                await _databaseService.DeleteItemAsync(user);
            }

            Members.Clear(); // Ook de lokale lijst in de ViewModel leegmaken
        }
    }
}