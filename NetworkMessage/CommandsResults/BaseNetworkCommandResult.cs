namespace NetworkMessage.CommandsResults
{
    public abstract class BaseNetworkCommandResult : INetworkObject
    {
        public virtual Type NetworkObjectType => GetType();

        [Newtonsoft.Json.JsonProperty]
        public string ErrorMessage { get; private set; }

        [Newtonsoft.Json.JsonProperty]
        public Exception Exception { get; private set; }
        
        public BaseNetworkCommandResult()
        {
        }

        public BaseNetworkCommandResult(string errorMessage, Exception exception = null)
        {
            if (string.IsNullOrEmpty(errorMessage)) throw new ArgumentNullException(nameof(errorMessage));
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public virtual string ToBase64()
        {
            return Convert.ToBase64String(ToByteArray());
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public virtual byte[] ToByteArray()
        {
            return System.Text.Encoding.UTF8.GetBytes(ToString());
        }

        public virtual Stream ToStream()
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(ToString()));
        }
    }
}
