using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLRat.Server
{
    public interface IServerUIHandler
    {
        void AddContext(MLRatContextEntry entry);
    }
}
