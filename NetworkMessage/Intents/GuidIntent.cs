using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    public class GuidIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public GuidIntent()
        {
            IntentType = nameof(GuidIntent);
        }

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateGuidCommand();
        }
    }
}
