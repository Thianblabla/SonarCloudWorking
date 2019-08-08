using System;

namespace OnlineService.Library
{
    // This can be expanded upon if needed

    /// <summary>
    /// Security flags for items.
    /// Can be combined using the | bitwise operator
    /// </summary>
    [Flags] public enum SecurityLevel : long
    {
        NONE = 0x0,
        FIREARM = 0x01,
        CHEMICAL = 0x02,
        EXPLOSIVE = 0x04,
        ALL = long.MinValue // max of an unsigned long
    }
}
