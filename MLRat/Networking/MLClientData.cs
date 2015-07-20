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
        public eSock.Server.eSockClient ClientSocket { get; private set; }
        public MLClientData(Guid _id, eSock.Server.eSockClient _socket)
        {
            ID = _id;
            Handshaken = false;
            ClientSocket = _socket;
        }
        
    }
}
