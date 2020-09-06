using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.VisualBasic;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using ReceiptScan.Command;
using ReceiptScan.Model;

namespace ReceiptScan.ViewModel
{
	/// <summary>
	/// Main view model of ReceiptScan application.
	/// </summary>
	public class ReceiptScanViewModel : ViewModelBase
	{
		#region Private fields and constants
		/// <summary>
		/// Image field.
		/// </summary>
		protected ImageSource imgSource;

		/// <summary>
		/// Command to handle camera
		/// </summary>
		protected DelegateCommand cameraCommand;

		/// <summary>
		/// State of camera, running or stop.
		/// </summary>
		protected bool isCameraRunning;

		/// <summary>
		/// Content of button to start and stop camera.
		/// </summary>
		protected string buttonContent; 

		/// <summary>
		/// Timer to update camera view.
		/// </summary>
		protected DispatcherTimer cameraTimer;

		/// <summary>
		/// FPS of camera.
		/// </summary>
		protected int fps;
		#endregion

		#region Constructors and the finalizer
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ReceiptScanViewModel()
		{
			this.fps = 30;

			this.MyCamera = new Camera(1280, 720);

			this.cameraTimer = new DispatcherTimer();
			int interval = (int)Math.Round((decimal)1000 / fps);
			cameraTimer.Interval = new TimeSpan(0, 0, 0, 0, interval);
			cameraTimer.Tick += new EventHandler(this.TimerEvent);

			this.buttonContent = "Start";
			this.isCameraRunning = false;
		}
		#endregion

		#region Public properties
		/// <summary>
		/// Image property.
		/// </summary>
		public ImageSource ImgSource
		{
			get
			{
				return this.imgSource;
			}
			set
			{
				this.imgSource = value;
				this.RaisePropertyChanged(nameof(ImgSource));
			}
		}

		/// <summary>
		/// Camera object.
		/// </summary>
		public Camera MyCamera { get; set; }
		#endregion

		#region Method
		/// <summary>
		/// Command of camera.
		/// </summary>
		public DelegateCommand CamereCommand
		{
			get
			{
				if (null == this.cameraCommand)
				{
					this.cameraCommand = new DelegateCommand(this.CameraCommandExecute);
				}
				return cameraCommand;
			}
		}

		/// <summary>
		/// Proeprty of camera button content.
		/// </summary>
		public string ButtonContent
		{
			get
			{
				return this.buttonContent;
			}
			set
			{
				this.buttonContent = value;
				this.RaisePropertyChanged(nameof(ButtonContent));
			}
		}

		/// <summary>
		/// Timer dispatcher of camera.
		/// </summary>
		/// <param name="sender">Not use</param>
		/// <param name="e">Not use</param>
		private void TimerEvent(object sender, EventArgs e)
		{
			this.ImgSource = this.MyCamera.Read();
		}

		/// <summary>
		/// Execute camera command to start or stop.
		/// </summary>
		public void CameraCommandExecute()
		{
			if (this.isCameraRunning)
			{
				this.cameraTimer.Stop();
				this.ButtonContent = "Start";
				this.isCameraRunning = false;
			}
			else if (!this.isCameraRunning)
			{
				this.cameraTimer.Start();
				this.ButtonContent = "Stop";
				this.isCameraRunning = true;
			}
			else
			{
				//This code must be unreachable.
			}
		}
		#endregion
	}
}
