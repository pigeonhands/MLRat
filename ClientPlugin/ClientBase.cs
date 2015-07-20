using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLRat.Client
{
    public interface IClientConnection
    {
        void Send(params object[] data);
    }
}
