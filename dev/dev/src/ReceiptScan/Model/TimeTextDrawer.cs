using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReceiptScan.Model
{
	/// <summary>
	/// Draw a time into a image.
	/// </summary>
	public class TimeTextDrawer : IImageDrawer
	{
		#region Implement interface.
		/// <summary>
		/// Draw time into a image.
		/// </summary>
		/// <param name="image">Image to write.</param>
		public void Draw(ref Mat image)
		{
			var curTime = DateTime.Now.ToString("HH:mm");
			image.PutText(curTime, new Point(5, 20), HersheyFonts.HersheyComplexSmall, 1, new Scalar(255, 255, 255), 1);
		}
		#endregion
	}
}
