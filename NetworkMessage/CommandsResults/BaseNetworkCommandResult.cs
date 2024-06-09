namespace NetworkMessage.CommandsResults
{
    public abstract class BaseNetworkCommandResult : INetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public string ErrorMessage { get; private set; }

        [Newtonsoft.Json.JsonProperty]
        public Exception Exception { get; private set; }
        
        public BaseNetworkCommandResult()
        {
        }

        public BaseNetworkCommandResult(string errorMessage, Exception exception = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage, nameof(errorMessage));
            ErrorMessage = errorMessage;
            Exception = exception;
        }

		public override string ToString()
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(this);
		}

		public virtual Stream ToStream()
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(ToString()));
        }
    }
}
