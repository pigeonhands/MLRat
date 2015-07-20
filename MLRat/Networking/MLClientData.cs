using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLRat.Networking
{
    public class MLClientData
    {
        public object DisplayObject { get; set; }
        public Guid ID { get; private set; }
        public bool Handshaken { get; set; }
        public MLClientData(Guid _id)
        {
            ID = _id;
            Handshaken = false;
        }
        
    }
}
