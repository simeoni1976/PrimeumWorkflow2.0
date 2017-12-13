namespace Workflow.Transverse.Helpers
{
    /// <summary>
    /// ProcessStepEnum enum.
    /// </summary>
    /// <remarks>
    /// This enum block permits to define a status list.
    /// </remarks>
    public enum ProcessStepEnum
    {
        PrevPropagate = 0,
        Init = 1,
        Act = 2,
        Constraint = 3,
        Restart = 4,
        Commit = 5,
        Propagate = 6,
        Finish = 7
    }
}