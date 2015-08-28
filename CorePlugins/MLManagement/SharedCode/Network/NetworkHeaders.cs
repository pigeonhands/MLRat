namespace SharedCode.Network
{
    public enum NetworkCommand : byte
    {
        TaskManager
    }

    public enum TaskManagerCommand : byte
    {
        GetProcesses,
        ProcessList
    }
}
