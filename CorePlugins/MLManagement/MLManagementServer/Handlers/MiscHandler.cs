using MLRat.Server;
using SharedCode.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLManagementServer.Handlers
{
    public static class MiscHandler
    {
        public static void CloseContextHandler(IClient[] clients)
        {
            foreach (IClient c in clients)
                c.Send((byte)NetworkCommand.Close);
        }
    }
}
