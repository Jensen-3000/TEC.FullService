namespace TEC.FullService.Shared.Common.Paging;

/// <summary>
/// Request that supports sorting by an enum.
/// </summary>
/// <typeparam name="TSortBy">Enum describing allowed sort fields.</typeparam>
public interface ISortRequest<TSortBy>
    where TSortBy : struct, Enum
{
    /// <summary>
    /// If null, the base list endpoint uses the enum default 0, which should be id.
    /// </summary>
    TSortBy? SortBy { get; set; }

    /// <summary>
    /// false = ascending, true = descending. 
    /// If null, the base list endpoint uses ascending.
    /// </summary>
    bool? Desc { get; set; }
}
