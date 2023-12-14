using NetworkMessage.Cryptography.AsymmetricCryptography;
using NetworkMessage.Cryptography.SymmetricCryptography;
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
        public Task<byte[]> ToByteArrayAsync(byte[] asymmetricPublicKey,
            IAsymmetricCryptographer asymmetricCryptographer,
            ISymmetricCryptographer symmetricCryptographer,
            CancellationToken token = default);
    }
}
