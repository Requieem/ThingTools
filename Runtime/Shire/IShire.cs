public interface IShire : IScriptableThing
{
    /// <summary>
    /// Indicates if the shire is currently being used.
    /// </summary>
    bool Used { get; set; }
    /// <summary>
    /// The order (possibly the list index) of this shire.
    /// </summary>
    int Order { get; set; }
}
