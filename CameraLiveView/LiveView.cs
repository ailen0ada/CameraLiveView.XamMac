using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using CoreGraphics;

namespace CameraLiveView
{
    public partial class LiveView : AppKit.NSView
    {
        #region Constructors

        // Called when created from unmanaged code
        public LiveView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public LiveView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public LiveView(CGRect frame) : base(frame)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        public override void DrawRect(CoreGraphics.CGRect dirtyRect)
        {
            base.DrawRect(dirtyRect);
            var context = NSGraphicsContext.CurrentContext.GraphicsPort;
            context.SetFillColor(0, 0, 0, 0.5f);
            context.FillRect(dirtyRect);

            this.Layer.BorderColor = NSColor.White.CGColor;
            this.Layer.BorderWidth = 2.0f;
        }

        public override void Layout()
        {
            base.Layout();
            if (Layer != null && Layer.Sublayers != null)
                foreach (var l in Layer.Sublayers)
                    l.Frame = this.Bounds;
        }
    }

}
