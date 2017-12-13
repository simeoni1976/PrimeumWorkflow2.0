using System;

namespace Workflow.Transverse.Helpers
{
    /// <summary>
    /// RightEnum enum.
    /// </summary>
    /// <remarks>
    /// This enum block permits to define a right list.
    /// </remarks>
    [Flags]
    public enum RightEnum
    {
        None = 0,
        Consultation = 1 << 0,
        Modification = 1 << 1,
        Validation   = 1 << 2
    }
}
