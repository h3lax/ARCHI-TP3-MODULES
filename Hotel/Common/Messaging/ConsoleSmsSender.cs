namespace Hotel.Common.Messaging;

public sealed class ConsoleSmsSender : ISmsSender
{
    public void Send(string message) =>
        Console.WriteLine($"  [SMS] {message}");
}
