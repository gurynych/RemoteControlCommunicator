using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.CommandsResults
{
    public class AmountOfRAMResult : NetworkCommandResultBase
    {
        public float AmountOfRAM { get; private set; }

        public AmountOfRAMResult(float amountOfRAM)
        {
            if (amountOfRAM == default) throw new ArgumentNullException(nameof(amountOfRAM));
            AmountOfRAM = amountOfRAM;
        }

        public override byte[] ToByteArray()
        {
            try
            {
                return BitConverter.GetBytes(AmountOfRAM);
            }
            catch (NullReferenceException)
            {
                throw;
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
