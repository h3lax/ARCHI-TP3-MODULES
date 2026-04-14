using Hotel.Common.Messaging;
using Hotel.HouseKeeping.Abstractions;
using Hotel.HouseKeeping.Domain;

namespace Hotel.Runner.Adapters;

public sealed class SmsCleaningNotifier : ICleaningNotifier
{
    private readonly ISmsSender _sms;

    public SmsCleaningNotifier(ISmsSender sms) => _sms = sms;

    public void NotifyNewTasks(List<CleaningTask> tasks)
    {
        foreach (var task in tasks)
        {
            _sms.Send(
                $"Housekeeping: {task.Type} for room {task.RoomId} on {task.Date:d}");
        }
    }
}
