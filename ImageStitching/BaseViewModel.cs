using System;
using System.ComponentModel;

namespace ImageStitching
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Fields

        #region Public Fields

        public event PropertyChangedEventHandler PropertyChanged; // INotifyPropertyChanged Member

        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields
        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties
        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods
        #endregion

        #region Protected Methods

        /// <summary>
        /// Call when a property value changes
        /// </summary>
        /// <param name="propertyName">Name of property which changed</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            // Make copy of handler to avoid thread issues
            PropertyChangedEventHandler handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Verifies the given property name corresponds to valid property
        /// </summary>
        /// <param name="propertyName">Name of property to verify</param>
        protected void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string errorMsg = "ViewModel does not contain property: " + propertyName;
                throw new ArgumentException("propertyName", errorMsg);
            }
        }

        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
