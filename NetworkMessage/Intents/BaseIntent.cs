using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    public abstract class BaseIntent : IIntent
    {
        public abstract string IntentType { get; protected set; }        

        public abstract INetworkCommand CreateCommand(ICommandFactory commandFactory);        
        
        public virtual Stream ToStream()
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(ToString()));
        }
    }
}
