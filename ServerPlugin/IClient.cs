using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLRat.Server
{
    public interface IClient
    {
        void Send(params object[] data);
        Guid ID {get; }
    }
}
