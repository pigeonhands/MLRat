using System;

namespace MLRat.Server
{
    public interface IClient
    {
        void Send(params object[] data);
        Guid ID {get; }
    }
}
