namespace NetworkMessage.CommandsResaults
{
    public class HwidResult : NetworkCommandResultBase
    {        
        public string Hwid { get; private set; }

        public HwidResult(string hwid)
        {
            if (hwid == default) throw new ArgumentNullException(nameof(hwid));
            Hwid = hwid;
        }         

        /*public override byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(hwid);
        }*/
    }
}
