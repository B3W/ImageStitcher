using ImageStitching.ImageDisplay.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageStitching.ImageDisplay.ViewModel
{
    public class ImageViewModel : BaseViewModel
    {
        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields

        private readonly ImageModel _imageModel;

        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        /// <summary>
        /// Image source
        /// </summary>
        public BitmapSource Image
        {
            get { return _imageModel.Image; }
        }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        public ImageViewModel(uint id, string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentException("Must provide valid path to image", "imagePath");
            }

            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"Path {imagePath} does not exist", imagePath);
            }

            // Load the image
            BitmapImage image = new BitmapImage();

            image.BeginInit();
            image.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            image.EndInit();

            _imageModel = new ImageModel(id, image, imagePath);
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
