
using NetworkMessage.CommandsResults;

namespace NetworkMessage.Commands
{
    public abstract class BaseNetworkCommand : INetworkObject
    {
        public Type NetworkObjectType => GetType();

        //For deserialization
        public BaseNetworkCommand()
        {            
        }

        public abstract Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects);

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