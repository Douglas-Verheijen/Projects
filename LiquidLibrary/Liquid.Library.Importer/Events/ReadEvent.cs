using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Library.Importer.Events
{
    public class ReadEventArgs : EventArgs
    {
        public virtual string[][] Data { get; set; }
    }
}
