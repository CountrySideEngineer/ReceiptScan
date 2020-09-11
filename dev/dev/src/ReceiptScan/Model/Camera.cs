using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Permissions;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ReceiptScan.Model
{
	/// <summary>
	/// Camera object.
	/// </summary>
	public class Camera
	{
		#region Private fields and constants
		/// <summary>
		/// Object of video capturing from camera
		/// </summary>
		protected VideoCapture capture;

		protected DispatcherTimer cameraTimer;
		#endregion

		#region Constructors and the finalizer
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Camera()
		{
			this.capture = new VideoCapture();
			this.capture.Open(0);

			this.cameraTimer = null;
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

			this.FrameWidth = width;
			this.FrameHeight = height;
			this.Fps = fps;

			if (this.capture.Open(0))
			{
				this.capture.FrameWidth = width;
				this.capture.FrameHeight = height;
				this.capture.Fps = fps;
			}

			this.cameraTimer = null;
		}
		#endregion

		#region Events
		/// <summary>
		/// Camera update event.
		/// </summary>
		public event CameraDataUpdateHandler CameraDataUpdateEvent;

		/// <summary>
		/// Camera event handler.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void CameraDataUpdateHandler(object sender, EventArgs e);
		#endregion

		#region Public properties
		/// <summary>
		/// Get the state a value indicates whether the timer is running or not.
		/// </summary>
		public bool IsEnabled
		{
			get
			{
				try
				{
					return this.cameraTimer.IsEnabled;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		public int Fps { get; protected set; }
		public int FrameWidth { get; protected set; }
		public int FrameHeight { get; protected set; }
		#endregion

		#region Other methods and private properties in calling order
		/// <summary>
		/// Start camera.
		/// </summary>
		/// <exception cref="InvalidOperationException">The camera can not run.</exception>
		public void Start()
		{
			if (!this.capture.IsOpened())
			{
				if (this.capture.Open(0))
				{
					this.capture.FrameHeight = this.FrameHeight;
					this.capture.FrameWidth = this.FrameWidth;
					this.capture.Fps = this.Fps;
				}
				else
				{
					throw new InvalidOperationException();
				}
			}

			if (null == this.cameraTimer)
			{
				int interval = (int)Math.Round((decimal)1000 / this.Fps);
				this.cameraTimer = new DispatcherTimer();
				this.cameraTimer.Interval = new TimeSpan(0, 0, 0, 0, interval);
				cameraTimer.Tick += new EventHandler(this.CameraTimerDispatcher);
			}
			this.cameraTimer.Start();
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

		/// <summary>
		/// Stop camera.
		/// </summary>
		public void Stop()
		{
			if (this.cameraTimer.IsEnabled)
			{
				this.cameraTimer.Stop();
			}
		}

		/// <summary>
		/// Callbakc method when the timer dispatched.
		/// </summary>
		/// <param name="sender">Event dispatcher.</param>
		/// <param name="e">Event args.</param>
		public void CameraTimerDispatcher(object sender, EventArgs e)
		{
			var newImgSource = this.Read();
			this.CameraDataUpdateEvent?.Invoke(this, new CameraEventArags(newImgSource));
		}
		#endregion
	}

	/// <summary>
	/// Event args of camera.
	/// </summary>
	public class CameraEventArags : EventArgs
	{
		#region Constructors and the finalizer
		/// <summary>
		/// Default constructor.
		/// </summary>
		public CameraEventArags()
		{
			this.ImgSource = null;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="imgSource">Image of camera to notify receiver.</param>
		public CameraEventArags(ImageSource imgSource)
		{
			this.ImgSource = imgSource;
		}
		#endregion

		#region Public properties
		/// <summary>
		/// Property of image.
		/// </summary>
		public ImageSource ImgSource { get; set; }
		#endregion
	}
}
