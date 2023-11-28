namespace NetworkMessage.Cryptography.Hash
{
    public interface IHashCreater
    {
        string GenerateSalt();

        string Hash(string data, string salt);

        string Hash(string data);
    }
}
