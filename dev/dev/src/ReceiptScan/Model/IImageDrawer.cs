using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReceiptScan.Model
{
	/// <summary>
	/// Interface of class to draw something like picture, text, etc...
	/// </summary>
	public interface IImageDrawer
	{
		#region Interface
		/// <summary>
		/// Draw a picture, text, or other something into a image.
		/// </summary>
		/// <param name="image">Image in Mat format to draw.</param>
		public void Draw(ref Mat image);
		#endregion
	}
}
