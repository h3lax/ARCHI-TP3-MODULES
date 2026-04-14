namespace Hotel.Common.Messaging;

public interface ISmsSender
{
    void Send(string message);
}
