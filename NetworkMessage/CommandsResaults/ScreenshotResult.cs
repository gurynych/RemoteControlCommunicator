using System.Drawing;

namespace NetworkMessage.CommandsResaults
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
            catch (NullReferenceException nullEx)
            {
                throw nullEx;
            }
            catch (NotSupportedException notSuppEx)
            {
                throw notSuppEx;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}