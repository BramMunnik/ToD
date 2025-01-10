using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToD.Model;

public class UserViewModel : INotifyPropertyChanged
{
    private ObservableCollection<User> _users;
    private User _currentUser;

    public ObservableCollection<User> Users
    {
        get => _users;
        set
        {
            _users = value;
            OnPropertyChanged();
        }
    }

    public User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnPropertyChanged();
        }
    }

    public UserViewModel()
    {
        _users = new ObservableCollection<User>();
    }

    public void AddUser(User user)
    {
        if (user != null && !string.IsNullOrWhiteSpace(user.Name))
        {
            _users.Add(user);
            OnPropertyChanged(nameof(Users));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
