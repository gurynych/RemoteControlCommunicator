namespace NetworkMessage.CommandsResults
{
    /// <summary>
    /// Класс, предоставляющий публичный ключ отправителя
    /// </summary>
    public class PublicKeyInfoResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public int PublicKeyLength { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private PublicKeyInfoResult()
        {            
        }

        public PublicKeyInfoResult(int publicKeyLength)
        {
            if (publicKeyLength <= 0) throw new ArgumentOutOfRangeException(nameof(publicKeyLength));
            PublicKeyLength = publicKeyLength;
        }

        public PublicKeyInfoResult(string errorMessage, Exception exception = null) 
            : base(errorMessage, exception)
        {
        }
    }
}
