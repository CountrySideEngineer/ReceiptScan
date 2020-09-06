using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ReceiptScan.Model
{
	/// <summary>
	/// Camera object.
	/// </summary>
	public class Camera
	{
		protected VideoCapture capture;
		protected Window window;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Camera()
		{
			this.capture = new VideoCapture();
			capture.Open(0);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="width">Width of camera image.</param>
		/// <param name="height">Height of camera image</param>
		/// <param name="fps">camera fps</param>
		public Camera(int width, int height, int fps = 60)
		{
			this.capture = new VideoCapture();
			capture.Open(0);
			capture.FrameWidth = width;
			capture.FrameHeight = height;
			capture.Fps = fps;
		}

		/// <summary>
		/// Read image from camera
		/// </summary>
		/// <returns>Image data read from camera in ImageSource object</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0063:単純な 'using' ステートメントを使用する", Justification = "<保留中>")]
		public ImageSource Read()
		{
			if (!this.capture.IsOpened())
			{
				return null;
			}

			var img = new Mat();
			this.capture.Read(img);
			if (img.Empty())
			{
				return null;
			}

			using (var memStream = new MemoryStream())
			{
				var bitMap = img.ToBitmap();
				bitMap.Save(memStream, ImageFormat.Bmp);
				memStream.Position = 0; //Set position to stream head.
				var bitMapImage = new BitmapImage();
				bitMapImage.BeginInit();
				bitMapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitMapImage.CreateOptions = BitmapCreateOptions.None;
				bitMapImage.StreamSource = memStream;   //The data is written into image...maybe.
				bitMapImage.EndInit();
				bitMapImage.Freeze();

				return bitMapImage;
			}
		}
	}
}
