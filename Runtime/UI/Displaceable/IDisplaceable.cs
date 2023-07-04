/// <summary>
/// Interface for objects that can be displaced.
/// </summary>
public interface IDisplaceable : IInitializable
{
    /// <summary>
    /// Previews the displacement of the object. Generally used in the Editor.
    /// </summary>
    void PreviewDisplacement();

    /// <summary>
    /// Displaces the object.
    /// </summary>
    void Displace();

    /// <summary>
    /// Resets the position of the object.
    /// </summary>
    void ResetPosition();

    /// <summary>
    /// Gets or sets the <see cref="ADisplaceable"/> behavior of the object.
    /// </summary>
    ADisplaceable DisplaceBehaviour { get; set; }
}
