using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    public class ScreenshotIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public ScreenshotIntent()
        {
            IntentType = nameof(ScreenshotIntent);
        }

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateScreenshotCommand();
        }
    }
}
