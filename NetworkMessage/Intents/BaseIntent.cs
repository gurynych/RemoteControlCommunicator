using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    public abstract class BaseIntent : INetworkObject
    {
        public abstract string IntentType { get; protected set; }

        public virtual Type NetworkObjectType => GetType();

        public abstract BaseNetworkCommand CreateCommand(ICommandFactory commandFactory);

        public string ToBase64()
        {
            return Convert.ToBase64String(ToByteArray());
        }

        public byte[] ToByteArray()
        {            
            return System.Text.Encoding.UTF8.GetBytes(ToString());
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
