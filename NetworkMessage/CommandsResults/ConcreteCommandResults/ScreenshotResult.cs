namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class ScreenshotResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public byte[] Image { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private ScreenshotResult()
        {
        }

        public ScreenshotResult(byte[] image)
        {
            if (image == null || image.Length == 0) throw new ArgumentNullException(nameof(image));
            Image = image;
        }

        public ScreenshotResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}