using System.Configuration;
using Newtonsoft.Json;

namespace NetworkMessage
{    
    public interface INetworkObject
    {
        public Stream ToStream();
    }
}
