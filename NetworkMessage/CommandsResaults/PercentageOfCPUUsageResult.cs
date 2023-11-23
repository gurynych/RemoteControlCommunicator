using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.CommandsResaults
{
    public class PercentageOfCPUUsageResult : NetworkCommandResultBase
    {
        public byte PercentageOfCPUUsage { get; private set; }

        public PercentageOfCPUUsageResult(byte persentageOfCPUUsage)
        {
            if (persentageOfCPUUsage == default) throw new ArgumentNullException(nameof(persentageOfCPUUsage));
            PercentageOfCPUUsage = persentageOfCPUUsage;
        }

        public override byte[] ToByteArray()
        {
            try
            {
                ByteConverter converter = new ByteConverter();
                return (byte[])converter.ConvertTo(PercentageOfCPUUsage, typeof(byte[]));
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
