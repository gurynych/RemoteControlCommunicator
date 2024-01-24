using System.Configuration;
using Newtonsoft.Json;

namespace NetworkMessage
{    
    public interface INetworkObject
    {        
        Type NetworkObjectType { get; }

        byte[] ToByteArray();

        string ToBase64();

        string ToString();

        public Stream ToStream();
    }
}
