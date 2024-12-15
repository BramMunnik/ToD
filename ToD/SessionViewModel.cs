using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToD
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _members;

        public ObservableCollection<string> Members
        {
            get { return _members; }
            set
            {
                _members = value;
                OnPropertyChanged();
            }
        }

        public SessionViewModel()
        {
            Members = new ObservableCollection<string>();
        }

        public void AddMember(string member)
        {
            if (!string.IsNullOrWhiteSpace(member))
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
