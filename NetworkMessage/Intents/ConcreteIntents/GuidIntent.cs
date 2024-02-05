using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class GuidIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public GuidIntent()
        {
            IntentType = nameof(GuidIntent);
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateGuidCommand();
        }
    }
}
