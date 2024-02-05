namespace NetworkMessage.Exceptions
{
    public class DeviceNotConnectedException : Exception
    {
        public DeviceNotConnectedException() : base("Device isn't connected") { }
    }
}
