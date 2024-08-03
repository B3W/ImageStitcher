using ImageStitching.Commands;
using ImageStitching.Main.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageStitching.Main.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields

        #region Public Fields

        public delegate void SaveImageEventHandler(object sender, EventArgs e);
        public event SaveImageEventHandler SaveImageEvent;

        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields

        /// <summary>
        /// Handle to the model
        /// </summary>
        private readonly MainModel _model;

        /// <summary>
        /// Keeps track of the next image ID
        /// </summary>
        private uint _nextImageId;

        private Bitmap _stichedImage;

        private BitmapSource _stichedImageDisplay;

        private bool _saveDialogVisible;

        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        public BitmapSource StitchedImageDisplay
        {
            get => _stichedImageDisplay;
            set
            {
                _stichedImageDisplay = value;
                OnPropertyChanged("StitchedImageDisplay");
            }
        }

        public bool SaveDialogVisible
        {
            get => _saveDialogVisible;
            set
            {
                _saveDialogVisible = value;
                OnPropertyChanged("SaveDialogVisible");
            }
        }

        public StitchDirection StitchDirection { get; set; }

        public bool CanChoose => _model.MainImage == null || _model.StitchImage == null;

        public RelayCommand StitchImagesCommand { get; private set; }

        public bool CanStitch => _model.MainImage != null && _model.StitchImage != null;

        public RelayCommand ConfirmSaveDialogCommand { get; private set; }

        public RelayCommand CloseSaveDialogCommand { get; private set; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties

        private Bitmap StitchedImage
        {
            get => _stichedImage;
            set
            {
                _stichedImage = value;

                if (value != null)
                {
                    StitchedImageDisplay = BitmapToBitmapSource(value);
                }
                else
                {
                    StitchedImageDisplay = null;
                }
            }
        }

        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        public MainViewModel()
        {
            _nextImageId = 0U;
            _stichedImage = null;
            _saveDialogVisible = false;
            _model = new MainModel();

            // Initialize commands
            StitchImagesCommand = new RelayCommand((param) => ExecuteImageStitchCommand(), (param) => CanStitch);
            ConfirmSaveDialogCommand = new RelayCommand((param) => ExecuteConfirmSaveDialogCommand());
            CloseSaveDialogCommand = new RelayCommand((param) => ExecuteCloseSaveDialogCommand());
        }

        
        public uint AddImage(string imagePath)
        {
            uint id = _nextImageId++;
            StitchImage image = new StitchImage(id, imagePath);

            // Set image relations
            if (_model.MainImage == null)
            {
                _model.MainImage = image;
            }
            else
            {
                _model.StitchImage = image;

                _model.MainImage.Right = image;
                image.Left = _model.MainImage;

                StitchDirection = StitchDirection.Horizontal;
            }

            OnPropertyChanged("CanChoose");

            return id;
        }


        public bool SaveImage(string savePath)
        {
            bool success = true;

            // Extract file extension
            string fileExt = null;

            try
            {
                fileExt = savePath.Substring(savePath.LastIndexOf('.'));
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine($"MainViewModel.SaveImage: Path '{savePath}' is invalid");
                success = false;
            }

            // Convert file extension to image format
            ImageFormat imageFormat = null;

            if (success)
            {
                imageFormat = ConvertExtensionToFormat(fileExt);
                success = imageFormat != null;
            }

            // Save image
            if (success)
            {
                try
                {
                    StitchedImage.Save(savePath, imageFormat);
                }
                catch (System.Runtime.InteropServices.ExternalException ex)
                {
                    Console.WriteLine($"MainViewModel.SaveImage: Exception when saving image to '{savePath}' - {ex.Message}");
                    success = false;
                }
            }

            return success;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods

        private void ExecuteImageStitchCommand()
        {
            // Calculate dimensions of stitched image
            int stitchedWidth;
            int stitchedHeight;

            if (StitchDirection == StitchDirection.Horizontal)
            {
                // Stiching horizontally
                stitchedWidth = _model.MainImage.Image.Width + _model.StitchImage.Image.Width;
                stitchedHeight = Math.Max(_model.MainImage.Image.Height, _model.StitchImage.Image.Height);
            }
            else
            {
                // Stiching vertically
                stitchedWidth = Math.Max(_model.MainImage.Image.Width, _model.StitchImage.Image.Width);
                stitchedHeight = _model.MainImage.Image.Height + _model.StitchImage.Image.Height;
            }

            // Bitmap to hold stiched image
            Bitmap stichedImage = new Bitmap(stitchedWidth, stitchedHeight);

            using (Graphics g = Graphics.FromImage(stichedImage))
            {
                // Set background color for stiched image
                g.Clear(System.Drawing.Color.Black);

                // Stitch images together
                Bitmap firstImage;
                Bitmap secondImage;
                int xOffset = 0;
                int yOffset = 0;

                if (_model.MainImage.Right != null || _model.MainImage.Down != null)
                {
                    firstImage = _model.MainImage.Image;
                    secondImage = _model.StitchImage.Image;
                }
                else
                {
                    firstImage = _model.StitchImage.Image;
                    secondImage = _model.MainImage.Image;
                }

                // Draw first image
                g.DrawImage(firstImage, xOffset, yOffset, firstImage.Width, firstImage.Height);

                // Offset for second image
                xOffset = (StitchDirection == StitchDirection.Horizontal) ? firstImage.Width : 0;
                yOffset = (StitchDirection == StitchDirection.Vertical) ? firstImage.Height : 0;

                // Draw second image
                g.DrawImage(secondImage, xOffset, yOffset, secondImage.Width, secondImage.Height);
            }

            // Display stiched image to user
            StitchedImage = stichedImage;
            SaveDialogVisible = true;
        }


        private void ExecuteConfirmSaveDialogCommand()
        {
            SaveImageEvent?.Invoke(this, EventArgs.Empty);
        }


        private void ExecuteCloseSaveDialogCommand()
        {
            StitchedImage = null;
            SaveDialogVisible = false;
        }


        /// <summary>
        /// Convert a System.Drawing.Bitmap object to a System.Windows.Media.Imaging.BitmapSource object
        /// </summary>
        /// <param name="bitmap">Bitmap to convert</param>
        /// <returns>BitmapSource representing original bitmap</returns>
        private static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                Console.WriteLine("MainViewModel.BitmapToBitmapSource: Null bitmap passed in");
                throw new ArgumentNullException("bitmap");
            }

            BitmapSource bmSrc;
            BitmapData bmpData = null;

            try
            {
                Rectangle bmpRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                bmpData = bitmap.LockBits(bmpRect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

                bmSrc = BitmapSource.Create(bitmap.Width,
                                            bitmap.Height,
                                            bitmap.HorizontalResolution,
                                            bitmap.VerticalResolution,
                                            PixelFormats.Bgra32,
                                            null,
                                            bmpData.Scan0,
                                            bmpData.Stride * bmpData.Height,
                                            bmpData.Stride);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MainViewModel.BitmapToBitmapSource: Exception converting Bitmap to BitmapSource - {ex.Message}");
                throw;
            }
            finally
            {
                bitmap?.UnlockBits(bmpData);
            }

            return bmSrc;
        }


        /// <summary>
        /// Conversion from file extension to image format
        /// </summary>
        /// <param name="extension">File extension to convert to image format</param>
        /// <returns>Image format represented by file extension</returns>
        private ImageFormat ConvertExtensionToFormat(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                Console.WriteLine("Null or empty file extension");
                throw new ArgumentNullException("extension", "File extension cannot be null or empty");
            }

            ImageFormat format;

            // Clean inputted string
            extension = extension.Trim('.');
            extension = extension.ToLower();

            // Determine extension for encoder
            if (extension.Equals("bmp"))
            {
                format = ImageFormat.Bmp;
            }
            else if (extension.Equals("gif"))
            {
                format = ImageFormat.Gif;
            }
            else if (extension.Equals("jpg") || extension.Equals("jpeg"))
            {
                format = ImageFormat.Jpeg;
            }
            else if (extension.Equals("png"))
            {
                format = ImageFormat.Png;
            }
            else if (extension.Equals("tif") || extension.Equals("tiff"))
            {
                format = ImageFormat.Tiff;
            }
            else
            {
                format = null;
            }

            return format;
        }

        #endregion

        #endregion // Methods
    }
}
