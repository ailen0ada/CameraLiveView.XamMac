using System;
using AppKit;
using Foundation;
using AVFoundation;
using System.Diagnostics;
using System.Linq;

namespace CameraLiveView
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            StartButton.Activated += StartButton_Activated;
            StopButton.Activated += StopButton_Activated;
            CaptureButton.Activated += CaptureButton_Activated;

            SetButtonState(false);
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();

            InitCaptureSession();

            SetupPreviewLayer();
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        private AVCaptureSession session;

        void InitCaptureSession()
        {
            session = new AVCaptureSession();
            if (session.CanSetSessionPreset(AVCaptureSession.PresetHigh))
            {
                session.SessionPreset = AVCaptureSession.PresetHigh;
            }
            var device = AVCaptureDevice
                .DevicesWithMediaType(AVMediaType.Video)
                .FirstOrDefault();
            if (device == null)
            {
                ShowAlert("Error", "No device was found.", NSAlertStyle.Critical);
                Environment.Exit(1);
            }

            NSError error;
            var input = new AVCaptureDeviceInput(device, out error);
            if (error != null)
            {
                ShowAlert("Error", error.ToString(), NSAlertStyle.Critical);
                Environment.Exit(1);
            }

            if (session.CanAddInput(input))
            {
                session.AddInput(input);
            }
            else
            {
                ShowAlert("Error", "Could not initialize session.", NSAlertStyle.Critical);
                Environment.Exit(1);
            }

            imageOutput = new AVCaptureStillImageOutput();
            imageOutput.OutputSettings = NSDictionary.FromObjectAndKey(AVVideo.CodecJPEG, AVVideo.CodecKey);
            if (session.CanAddOutput(imageOutput))
            {
                session.AddOutput(imageOutput);
            }
        }

        private AVCaptureVideoPreviewLayer previewLayer;

        private AVCaptureStillImageOutput imageOutput;

        void SetupPreviewLayer()
        {
            previewLayer = new AVCaptureVideoPreviewLayer(session);
            previewLayer.VideoGravity = AVLayerVideoGravity.ResizeAspect;
            previewLayer.Frame = LiveView.Bounds;
            LiveView.Layer.AddSublayer(previewLayer);
        }

        void StartButton_Activated(object sender, EventArgs e)
        {
            if (!session.Running)
                session.StartRunning();

            SetButtonState(session.Running);
        }

        void StopButton_Activated(object sender, EventArgs e)
        {
            if (session.Running)
                session.StopRunning();
            SetButtonState(session.Running);
        }

        async void CaptureButton_Activated(object sender, EventArgs e)
        {
            var connection = imageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var buf = await imageOutput.CaptureStillImageTaskAsync(connection);
            if (buf == null)
                return;

            var imageData = AVCaptureStillImageOutput.JpegStillToNSData(buf);

            var fn = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                $"{DateTime.Now:yyyyMMdd_HHmmss}.jpg");

            System.IO.File.WriteAllBytes(fn, imageData.ToArray());
        }

        void SetButtonState(bool isStarted)
        {
            StartButton.Enabled = !isStarted;
            StopButton.Enabled = isStarted;
            CaptureButton.Enabled = isStarted;
        }

        void ShowAlert(string title, string message, NSAlertStyle style)
        {
            using (var alert = new NSAlert())
            {
                alert.MessageText = title;
                alert.InformativeText = message;
                alert.AlertStyle = style;
                alert.RunSheetModal(View.Window);
            }
        }
    }
}