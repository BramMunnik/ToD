using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using ToD.ViewModel;

namespace ToD
{
    public partial class GamePage : ContentPage
    {
        private GameViewModel _viewModel;

        public GamePage(ObservableCollection<string> members, bool useApiQuestions)
        {
            InitializeComponent();
            
            _viewModel = new GameViewModel(members, useApiQuestions);

            BindingContext = _viewModel;
        }
    }
}