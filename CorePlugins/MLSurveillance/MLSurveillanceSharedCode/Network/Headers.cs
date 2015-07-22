namespace MLSurveillanceSharedCode.Network
{
    public enum NetworkCommand : byte
    {
        RemoteChat
    }

    public enum RemoteChatCommand : byte
    {
        Start,
        Stop,
        Message
    }
}
