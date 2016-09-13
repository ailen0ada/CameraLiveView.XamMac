// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace CameraLiveView
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton CaptureButton { get; set; }

		[Outlet]
		CameraLiveView.LiveView LiveView { get; set; }

		[Outlet]
		AppKit.NSButton StartButton { get; set; }

		[Outlet]
		AppKit.NSButton StopButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CaptureButton != null) {
				CaptureButton.Dispose ();
				CaptureButton = null;
			}

			if (StopButton != null) {
				StopButton.Dispose ();
				StopButton = null;
			}

			if (StartButton != null) {
				StartButton.Dispose ();
				StartButton = null;
			}

			if (LiveView != null) {
				LiveView.Dispose ();
				LiveView = null;
			}
		}
	}
}
