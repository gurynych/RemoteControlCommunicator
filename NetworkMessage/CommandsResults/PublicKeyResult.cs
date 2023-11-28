namespace NetworkMessage.CommandsResults
{
    /// <summary>
    /// Класс, предоставляющий публичный ключ отправителя
    /// </summary>
    public class PublicKeyResult : NetworkCommandResultBase
    {
        public byte[] PublicKey { get; private set; }

        public PublicKeyResult(byte[] publicKey)
        {
            if (publicKey == default) throw new ArgumentNullException(nameof(publicKey));
            PublicKey = publicKey;
        }        

        /*public override byte[] ToByteArray()
        {
            return publicKey;
        }*/
    }
}
