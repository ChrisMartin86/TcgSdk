namespace TcgSdk.Common
{
    public interface ITcgSdkObject
    {
        string Name { get; }

        TcgSdkResponseType ResponseType { get; }

        string ToString();
    }
}
