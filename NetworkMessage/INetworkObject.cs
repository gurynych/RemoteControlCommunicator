using System.Configuration;

namespace NetworkMessage
{
    public interface INetworkObject
    {        
        Type NetworkObjectType { get; }

        byte[] ToByteArray();

        string ToBase64();
    }
}
