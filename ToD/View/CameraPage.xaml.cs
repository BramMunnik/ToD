using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using ToD.ViewModel;

namespace ToD
{
    public partial class CameraPage : ContentPage
    {
        private CameraViewModel _viewModel;

        public CameraPage()
        {
            InitializeComponent();
            _viewModel = new CameraViewModel();
            BindingContext = _viewModel;
        }
    }
}
