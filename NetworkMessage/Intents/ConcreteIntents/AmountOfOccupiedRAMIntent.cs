﻿using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class AmountOfOccupiedRAMIntent : BaseIntent
    {   
        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateAmountOfOccupiedRAMCommand();
        }
    }
}
