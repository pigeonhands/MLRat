namespace SharedCode.Network
{
    public enum NetworkCommand : byte
    {
        TaskManager,
        Ping,
        Pong
    }

    public enum TaskManagerCommand : byte
    {
        GetProcesses,
        ProcessList
    }
}
