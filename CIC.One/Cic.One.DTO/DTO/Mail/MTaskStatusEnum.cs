namespace Cic.One.DTO
{
    public enum MTaskStatusEnum
    {
        // Summary:
        //     The execution of the task is not started.
        NotStarted = 0,

        //
        // Summary:
        //     The execution of the task is in progress.
        InProgress = 1,

        //
        // Summary:
        //     The execution of the task is completed.
        Completed = 2,

        //
        // Summary:
        //     The execution of the task is waiting on others.
        WaitingOnOthers = 3,

        //
        // Summary:
        //     The execution of the task is deferred.
        Deferred = 4,
    }
}