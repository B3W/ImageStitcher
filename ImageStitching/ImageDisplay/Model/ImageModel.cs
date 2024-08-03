using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageStitching.ImageDisplay.Model
{
    public class ImageModel
    {
        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields
        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        /// <summary>
        /// Image ID
        /// </summary>
        public uint Id { get; }

        /// <summary>
        /// Image source
        /// </summary>
        public BitmapSource Image { get; }

        /// <summary>
        /// Path where image was loaded from
        /// </summary>
        public string ImagePath { get; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        public ImageModel(uint id, BitmapSource image, string imagePath)
        {
            Id = id;
            Image = image;
            ImagePath = imagePath;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
