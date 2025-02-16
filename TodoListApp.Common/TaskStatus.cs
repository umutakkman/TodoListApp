namespace TodoListApp.Common;

/// <summary>
/// Represents the status of a task in the to-do list application.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// The task has not been started yet.
    /// </summary>
    NotStarted,

    /// <summary>
    /// The task is currently in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// The task has been completed.
    /// </summary>
    Completed
}
