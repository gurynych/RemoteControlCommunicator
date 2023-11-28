namespace NetworkMessage.CommandsResults
{
    public class DeviceGuidResult : NetworkCommandResultBase
    {        
        public string Guid { get; private set; }

        public DeviceGuidResult(string guid)
        {
            if (guid == default) throw new ArgumentNullException(nameof(guid));
            Guid = guid;
        }         

        /*public override byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(hwid);
        }*/
    }
}
