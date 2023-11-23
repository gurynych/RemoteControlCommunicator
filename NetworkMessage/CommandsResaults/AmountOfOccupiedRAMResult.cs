using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.CommandsResaults
{
    public class AmountOfOccupiedRAMResult : NetworkCommandResultBase
    {
        public float AmountOfOccupiedRAM { get; private set; }

        public AmountOfOccupiedRAMResult(float amountOfOccupiedRAM)
        {
            AmountOfOccupiedRAM = amountOfOccupiedRAM;
        }

        public override byte[] ToByteArray()
        {
            try
            {
                return BitConverter.GetBytes(AmountOfOccupiedRAM);
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
