using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class User
{
    public string Name { get; set; }
    public Guid TemporaryId { get; set; }
    public Preferences Preferences { get; set; }
}

public class Preferences
{
    public List<string> SelectedCategories { get; set; }
    public int RiskLevel { get; set; }  // 1-5 scale
}


public class UserViewModel : INotifyPropertyChanged
{
    private ObservableCollection<User> _users;
    private User _currentUser;

    public ObservableCollection<User> Users
    {
        get { return _users; }
        set
        {
            _users = value;
            OnPropertyChanged();
        }
    }

    public User CurrentUser
    {
        get { return _currentUser; }
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
        _users.Add(user);
        OnPropertyChanged(nameof(Users));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
