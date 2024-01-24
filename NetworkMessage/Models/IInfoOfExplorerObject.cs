using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Models
{
    internal interface IInfoOfExplorerObject
    {
        public string Name { get; }

        public DateTime CreationDate { get; }

        public DateTime ChangingDate { get; }

        public string FullName { get; }
    }
}
