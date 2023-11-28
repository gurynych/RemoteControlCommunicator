using System.Drawing;

namespace NetworkMessage.CommandsResults
{
    public class ScreenshotResult : NetworkCommandResultBase
    {
        public Bitmap Bitmap { get; private set; }

        public ScreenshotResult(Bitmap bitmap)
        {
            if (bitmap == default) throw new ArgumentNullException(nameof(bitmap));
            Bitmap = bitmap;
        }

        public override byte[] ToByteArray()
        {
            try
            {
                ImageConverter converter = new ImageConverter();
                return (byte[])converter.ConvertTo(Bitmap, typeof(byte[]));
                //throw new NotImplementedException();
            }
            catch (NullReferenceException)
            {
                throw;
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}