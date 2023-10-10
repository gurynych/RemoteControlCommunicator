using System.Drawing;
using System.Windows.Forms;
using NetworkMessage.CommandsResaults;

namespace NetworkMessage.Commands
{
    public class ScreenshonCommand : NetworkCommandBase
    {
        public override Task<INetworkCommandResult> Do(params object[] objects)
        {
            int width = Screen.AllScreens.Sum(s => s.Bounds.Width);
            int height = Screen.AllScreens.Max(s => s.Bounds.Height);
            using Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, new Size(width, height));
            }

            INetworkCommandResult screenshot = new ScreenshotResult(bitmap);
            return Task.FromResult(screenshot);
        }        
    }
}