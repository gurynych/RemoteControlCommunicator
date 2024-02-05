namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class PublicKeyResult : BaseNetworkCommandResult
    {
        public byte[] PublicKey { get; private set; }

        public PublicKeyResult(byte[] publicKey)
        {
            if (publicKey == null || publicKey.Length == 0) throw new ArgumentNullException(nameof(publicKey));
            PublicKey = publicKey;
        }        

        public override Stream ToStream()
        {
            return new MemoryStream(PublicKey);
        }
    }
}
