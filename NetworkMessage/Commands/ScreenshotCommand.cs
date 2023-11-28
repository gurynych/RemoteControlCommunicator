using NetworkMessage.CommandsResults;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NetworkMessage.Commands
{
    public class ScreenshotCommand : NetworkCommandBase
    {
        public override Task<NetworkCommandResultBase> DoAsync(CancellationToken token = default,params object[] objects)
        { 
            //throw new NotImplementedException();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                int width = Screen.AllScreens.Sum(s => s.Bounds.Width);
                int height = Screen.AllScreens.Max(s => s.Bounds.Height);
                using Bitmap bitmap = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, new Size(width, height));
                }

                NetworkCommandResultBase screenshot = new ScreenshotResult(bitmap);
                return Task.FromResult(screenshot);
            }

            return default;
        }
    }
}