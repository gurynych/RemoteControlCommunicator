namespace NetworkMessage.CommandsResults.ConcreteCommandResults
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
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(publicKeyLength);
            PublicKeyLength = publicKeyLength;
        }

        public PublicKeyInfoResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
