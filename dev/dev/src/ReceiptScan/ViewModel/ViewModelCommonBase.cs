using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ReceiptScan.ViewModel
{
	public class ViewModelCommonBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify property changed.
        /// </summary>
        /// <param name="propertyName">Name property whose value changed.</param>
        protected virtual void RaisePropertyChanged(String propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
