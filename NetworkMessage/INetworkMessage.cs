using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage
{
    public interface INetworkMessage
    {
        byte[] EncryptedSymmetricKey { get; set; }

        byte[] EncryptedIV { get; set; }

        byte[] EncryptedNetworkObject { get; set; }

        string ToString();

        byte[] ToByteArray();
    }
}
