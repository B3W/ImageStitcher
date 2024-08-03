using ImageStitching.ImageDisplay.View;
using ImageStitching.Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageStitching.Main.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            MainViewModel viewModel = new MainViewModel();
            DataContext = viewModel;

            viewModel.SaveImageEvent += SaveImageEventHandler;
            ImagePickerBtn.Click += ImagePickerBtn_Click;
        }

        private void ImagePickerBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get the full path for the initial save directory (relative paths will throw exception)
            string initialPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            // Configure save dialog
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = ".bmp",
                Filter = "BMP (.bmp)|*.bmp|GIF (.gif)|*.gif|JPEG (.jpg)|*.jpg;*.jpeg|PNG (.png)|*.png|TIFF (.tif)|*.tif;*.tiff|WMP (.wmp)|*.wmp",
                InitialDirectory = initialPath,
                Multiselect = false,    // TODO: Add support
                Title = "Select Image"
            };

            // Show dialog
            bool? result = openDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    // Relay new image to view model
                    uint id = (DataContext as MainViewModel).AddImage(openDialog.FileName);

                    // Create image control for this image
                    ImageView imageControl = new ImageView(id, openDialog.FileName);

                    // TODO: Add image with appropriate relations
                    _ = ImagePanel.Children.Add(imageControl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"MainView.ImagePickerBtn_Click: Exception while loading image - {ex.Message}");
                    // TODO: Show notification
                }
            }
        }

        private void SaveImageEventHandler(object sender, EventArgs e)
        {
            // Get the full path for the initial save directory (relative paths will throw exception)
            string initialPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            // Configure save dialog
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = ".bmp",
                FileName = "stitch",
                Filter = "BMP (.bmp)|*.bmp|GIF (.gif)|*.gif|JPEG (.jpg)|*.jpg;*.jpeg|PNG (.png)|*.png|TIFF (.tif)|*.tif;*.tiff",
                InitialDirectory = initialPath,
                OverwritePrompt = true,
                Title = "Save Image"
            };

            // Show dialog
            bool? result = saveDialog.ShowDialog();

            if (result == true)
            {
                // Save to file
                if (!(DataContext as MainViewModel).SaveImage(saveDialog.FileName))
                {
                    // TODO: Show notification
                }
            }
        }
    }
}
