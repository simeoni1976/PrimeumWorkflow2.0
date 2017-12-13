namespace Workflow.Transverse.Helpers
{
    /// <summary>
    /// SelectorStateEnum enum.
    /// </summary>
    /// <remarks>
    /// This enum block permits to define a Selector status list.
    /// </remarks>
    public enum SelectorStateEnum
    {
        Void = 0,
        Create = 1,
        PrevPropagate = 2,
        Init = 3,
        Modify = 4,
        Act = 5,
        Constraint = 6,
        Validate = 7,
        Restart = 8,
        Commit = 9,
        Propagate = 10,
        Finish = 11
    }

}