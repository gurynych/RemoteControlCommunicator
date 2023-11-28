namespace NetworkMessage.Cryptography.Hash
{
    public class BCryptCreater : IHashCreater
    {
        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }

        public string Hash(string data, string salt)
        {
            if (data == default) throw new ArgumentNullException(nameof(data));
            if (salt == default) throw new ArgumentNullException(nameof(salt));
            return BCrypt.Net.BCrypt.HashPassword(data, salt);
        }

        public string Hash(string data)
        {
            if (data == default) throw new ArgumentNullException(nameof(data));
            return BCrypt.Net.BCrypt.HashPassword(data);
        }
    }
}
