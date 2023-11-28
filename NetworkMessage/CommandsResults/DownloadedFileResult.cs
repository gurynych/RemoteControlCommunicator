using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.CommandsResults
{
    public class DownloadedFileResult : NetworkCommandResultBase
    {
       public byte[] File {  get; private set; }

       public DownloadedFileResult(byte[] file) 
       {
            File = file;
       }

    }
}
