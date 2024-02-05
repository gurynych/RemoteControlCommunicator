namespace NetworkMessage.Models
{
    public readonly struct ConfigurationData
    {
        public byte[] SymKey { get; }

        public byte[] SymIV { get; }

        public long MessageSize { get; }

        public ConfigurationData(byte[] symKey, byte[] symIV, long messageSize)
        {
            SymKey = symKey;
            SymIV = symIV;
            MessageSize = messageSize;
        }        
    }
}
