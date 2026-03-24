namespace Hotel;

public class SmsSender : ICleaningNotifier
{
    public void NotifyNewTasks(List<CleaningTask> tasks)
    {
        foreach (var task in tasks)
        {
            Console.WriteLine($"  [SMS] Housekeeping: {task.Type} for room {task.RoomId} on {task.Date:d}");
        }
    }
}
