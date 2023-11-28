using NetworkMessage.CommandsResults;

namespace NetworkMessage.Commands
{
    public abstract class NetworkCommandBase : INetworkObject
    {
        public Type NetworkObjectType => GetType();

        public abstract Task<NetworkCommandResultBase> DoAsync(CancellationToken token = default, params object[] objects);

        public string ToBase64()
        {
            return Convert.ToBase64String(ToByteArray());
        }

        public override string ToString()
        {            
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public byte[] ToByteArray()
        {
            return System.Text.Encoding.UTF8.GetBytes(ToString());
        }
    }
}