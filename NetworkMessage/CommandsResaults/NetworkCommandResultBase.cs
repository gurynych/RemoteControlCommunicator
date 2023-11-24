﻿namespace NetworkMessage.CommandsResaults
{
    public abstract class NetworkCommandResultBase : INetworkCommandResult
    {
        public virtual Type NetworkObjectType => GetType();

        public virtual string ToBase64()
        {
            return Convert.ToBase64String(ToByteArray());
        }

        public override string ToString()
        {
            var a = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public virtual byte[] ToByteArray()
        {
            return System.Text.Encoding.UTF8.GetBytes(ToString());
        }
    }
}
