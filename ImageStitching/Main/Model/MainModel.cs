using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageStitching.Main.Model
{
    public class MainModel
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
        /// Main image at center of stitching view
        /// </summary>
        public StitchImage MainImage { get; set; }

        /// <summary>
        /// Image to stitch to the main image
        /// </summary>
        public StitchImage StitchImage { get; set; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        public MainModel()
        {
            MainImage = null;
            StitchImage = null;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
