using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using ToD.ViewModel;

namespace ToD
{
    public partial class GamePage : ContentPage
    {
        private GameViewModel _viewModel;

        // Modify the constructor to accept 'members' and 'useApiQuestions'
        public GamePage(ObservableCollection<string> members, bool useApiQuestions)
        {
            InitializeComponent();

            // Initialize the GameViewModel with both parameters
            _viewModel = new GameViewModel(members, useApiQuestions);

            // Set the BindingContext for data-binding
            BindingContext = _viewModel;
        }
    }
}