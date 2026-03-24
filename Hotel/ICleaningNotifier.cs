namespace Hotel;

public interface ICleaningNotifier
{
    void NotifyNewTasks(List<CleaningTask> tasks);
}
