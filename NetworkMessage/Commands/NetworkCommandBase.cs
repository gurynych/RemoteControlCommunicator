using NetworkMessage.CommandsResaults;
using Newtonsoft.Json;
using System.Text;

namespace NetworkMessage.Commands
{
    public abstract class NetworkCommandBase : INetworkCommand
    {
        public Type NetworkObjectType => GetType();

        public abstract Task<INetworkCommandResult> Do(params object[] objects);

        public string ToBase64()
        {
            return Convert.ToBase64String(ToByteArray());
        }

        public byte[] ToByteArray()
        {
            string json = JsonConvert.SerializeObject(this);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}