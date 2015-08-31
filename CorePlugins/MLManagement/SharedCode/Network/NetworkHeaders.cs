namespace SharedCode.Network
{
    public enum NetworkCommand : byte
    {
        TaskManager,
        Ping,
        Pong,
        RegistryEdit
    }

    public enum TaskManagerCommand : byte
    {
        GetProcesses,
        ProcessList
    }

    public enum RegistryCommand : byte
    {
        UpdateNodes,
        UpdateNodeResponce,
        UpdateKeys,
        EmptyNode,
        ValueResponce,
        NodeDeniedAccess,
        ValueDeniedAccess,
        SetValue,
        DeleteValue
    }
    public enum RegistryKeyType :byte
    {
        None,
        CurrentUser,
        LocalMachine,
        ClassesRoot,
        UserRoot,
        CurrentConfig
    }

}
