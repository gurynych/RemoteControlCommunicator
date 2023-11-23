using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Cryptography
{
    public interface ICryptographer
    {
        byte[] Encrypt(byte[] data, byte[] key);

        byte[] Decrypt(byte[] encryptedData, byte[] key);
    }
}
