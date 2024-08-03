using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageStitching.Main.Model
{
    public enum StitchDirection
    {
        Horizontal,
        Vertical
    }


    public class StitchImage
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

        
        public uint Id { get; }


        public string ImagePath { get; }


        public Bitmap Image { get; }


        public StitchImage Up { get; set; }


        public StitchImage Down { get; set; }


        public StitchImage Left { get; set; }


        public StitchImage Right { get; set; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods


        public StitchImage(uint id, string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentException("Must provide valid path to image", "imagePath");
            }

            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"Path {imagePath} does not exist", imagePath);
            }

            Id = id;
            ImagePath = imagePath;

            // Load image
            Image = (Bitmap)System.Drawing.Image.FromFile(imagePath);

            // Initialize image relations
            Up = null;
            Down = null;
            Left = null;
            Right = null;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
