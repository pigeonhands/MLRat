namespace SharedCode.Network
{
    public enum NetworkCommand : byte
    {
        TaskManager,
        Ping,
        Pong,
        RegistryEdit,
        FileManager,
        Execute,
        ExecuteHidden,
        Close,
        DeleteFile,
        DownloadAndExecute,
        DownloadFile,
        KillProcess,
        SuspendProcess,
        ResumeProcess
    }

    public enum FileManagerCommand : byte
    {
        DriveResponce,
        DirectoryResponce,
        Update,
        Invalid,
        StartDownload,
        DownloadInvalid,
        DownloadBlock,
        MoveFile,
        ForceMoveFile,
        CopyFile,
        RenameFile,
        MoveResponce,
        CopyResponce,
        GetFileProperties,
        PropertiesResponce,
        PropertiesFailed
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
