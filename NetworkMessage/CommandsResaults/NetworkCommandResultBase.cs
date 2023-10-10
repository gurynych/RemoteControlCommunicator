using Newtonsoft.Json;
using System.Text;

namespace NetworkMessage.CommandsResaults
{
    public abstract class NetworkCommandResultBase : INetworkCommandResult
    {
        public virtual Type NetworkObjectType => GetType();

        public virtual string ToBase64()
        {
            return Convert.ToBase64String(ToByteArray());
        }

        public virtual byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));            
        }
    }
}
