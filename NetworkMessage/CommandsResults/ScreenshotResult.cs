namespace NetworkMessage.CommandsResults
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
            Image = image ?? throw new ArgumentNullException(nameof(image));
        }

        public ScreenshotResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}