using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class ScreenshotIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public ScreenshotIntent()
        {
            IntentType = nameof(ScreenshotIntent);
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateScreenshotCommand();
        }
    }
}
