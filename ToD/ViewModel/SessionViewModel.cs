using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToD.ViewModel
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _members;

        public ObservableCollection<string> Members { get; set; } = new ObservableCollection<string>();


        public SessionViewModel()
        {
            Members = new ObservableCollection<string>();
        }

        public void AddMember(string member)
        {
            if (!string.IsNullOrWhiteSpace(member) && !Members.Contains(member))
            {
                Members.Add(member);
            }
        }

        public void RemoveMember(string member)
        {
            if (Members.Contains(member))
            {
                Members.Remove(member);
            }
        }

        public void EditMember(string oldName, string newName)
        {
            var index = Members.IndexOf(oldName);

            if (index != -1 && !string.IsNullOrWhiteSpace(newName))
            {
                if (Members.Contains(newName))
                {
                    throw new InvalidOperationException("De nieuwe naam bestaat al in de lijst.");
                }

                Members[index] = newName;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
