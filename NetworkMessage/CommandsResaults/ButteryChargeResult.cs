using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.CommandsResaults
{
    public class ButteryChargeResult : NetworkCommandResultBase
    {
        public byte ButteryChargePercent { get; private set; }

        public ButteryChargeResult(byte butteryChargePercent) 
        {
            if (butteryChargePercent == default) throw new ArgumentNullException(nameof(butteryChargePercent));
            ButteryChargePercent = butteryChargePercent;
        }

        public override byte[] ToByteArray()
        {
            try
            {
                ByteConverter converter = new ByteConverter();
                return (byte[])converter.ConvertTo(ButteryChargePercent, typeof(byte[]));
            }
            catch (NullReferenceException nullEx)
            {
                throw nullEx;
            }
            catch (NotSupportedException notSuppEx)
            {
                throw notSuppEx;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
