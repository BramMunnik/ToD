using Microsoft.Maui.Storage;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using System.Windows.Input;

namespace ToD.ViewModel
{
    public class CameraViewModel : BindableObject
    {
        private string _photoPath;
        public string PhotoPath
        {
            get => _photoPath;
            set
            {
                _photoPath = value;
                OnPropertyChanged();
            }
        }

        public ICommand CapturePhotoCommand { get; }

        public CameraViewModel()
        {
            CapturePhotoCommand = new Command(async () => await CapturePhotoAsync());
        }

        // Method to capture a photo using the camera
        private async Task CapturePhotoAsync()
        {
            try
            {
                // Capture the photo using the MediaPicker
                var photo = await MediaPicker.CapturePhotoAsync();

                // Save the photo to a local path
                var newFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }

                // Update the PhotoPath property so the image can be displayed
                PhotoPath = newFilePath;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle exception if camera is not supported on the device
                await Application.Current.MainPage.DisplayAlert("Error", "Camera is not supported on this device.", "OK");
            }
            catch (PermissionException pEx)
            {
                // Handle permissions error
                await Application.Current.MainPage.DisplayAlert("Error", "Camera permissions are required.", "OK");
            }
            catch (Exception ex)
            {
                // General error handling
                await Application.Current.MainPage.DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }
    }
}