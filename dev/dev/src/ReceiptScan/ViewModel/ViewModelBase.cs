using System;
using System.Collections.Generic;
using System.Text;

namespace ReceiptScan.ViewModel
{
	public class ViewModelBase : ViewModelCommonBase
	{
        /// <summary>
        /// Action to notify the result ok.
        /// </summary>
        public Action<string, string> NotifyOk { get; set; }

        /// <summary>
        /// Action to notify the result ng.
        /// </summary>
        public Action<string, string> NotifyNg { get; set; }
    }
}
