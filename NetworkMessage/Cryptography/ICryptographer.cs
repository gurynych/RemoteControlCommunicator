using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Cryptography
{
    public interface ICryptographer
    {
        Task<byte[]> Encrypt(byte[] data, byte[] key, CancellationToken token = default);

        Task<byte[]> Decrypt(byte[] encryptedData, byte[] key, CancellationToken token = default);
    }
}
