﻿
using NetworkMessage.CommandsResults;
using Newtonsoft.Json;

namespace NetworkMessage.Commands
{
    public abstract class BaseNetworkCommand : INetworkCommand
    {        
        //For deserialization
        public BaseNetworkCommand()
        {            
        }

        public abstract Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects);

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