using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;
using Newtonsoft.Json;

namespace NetworkMessage.Intents
{
    public abstract class BaseIntent : IIntent
    {
        public virtual string IntentType { get; protected set; }

        public BaseIntent()
        {
			IntentType = GetType().Name;
		}

        public abstract INetworkCommand CreateCommand(ICommandFactory commandFactory);

		public override string ToString()
		{
            return JsonConvert.SerializeObject(this);
		}

		public virtual Stream ToStream()
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(ToString()));
        }
    }
}
