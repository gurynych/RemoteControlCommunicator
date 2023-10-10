using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Text;

namespace NetworkMessage.CommandsResaults
{
    public class MacAddressResult : NetworkCommandResultBase
    {
        public string MacAddress { get; private set; }

        public MacAddressResult(string macAddress)
        {
            if (macAddress == default) throw new ArgumentNullException(nameof(macAddress));
            MacAddress = macAddress;
        }


        /*public override byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
            //return Encoding.UTF8.GetBytes(macAddress);
        }*/
    }
}
