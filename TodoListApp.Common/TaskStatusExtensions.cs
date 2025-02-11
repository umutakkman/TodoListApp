namespace TodoListApp.Common;
public static class TaskStatusExtensions
{
    public static bool IsCompleted(this TaskStatus status)
    {
        ArgumentNullException.ThrowIfNull(status);
        switch (status)
        {
            case TaskStatus.Completed:
                return true;
            case TaskStatus.NotStarted:
                return false;
            case TaskStatus.InProgress:
                return false;
            default:
                return false;
        }
    }
}
