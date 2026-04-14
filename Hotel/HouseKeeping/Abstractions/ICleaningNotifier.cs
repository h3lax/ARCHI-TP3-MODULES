using Hotel.HouseKeeping.Domain;

namespace Hotel.HouseKeeping.Abstractions;

public interface ICleaningNotifier
{
    void NotifyNewTasks(List<CleaningTask> tasks);
}
