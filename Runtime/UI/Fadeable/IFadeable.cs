/// <summary>
/// Interface for objects that can apply fade effects.
/// </summary>
public interface IFadeable : IInitializable
{
    /// <summary>
    /// Previews the fade effect. Generally used in the editor.
    /// </summary>
    void PreviewFade();

    /// <summary>
    /// Initiates the fade effect.
    /// </summary>
    void Fade();

    /// <summary>
    /// Resets the fade effect.
    /// </summary>
    void ResetFade();

    /// <summary>
    /// The fade behavior associated with the IFadeable object.
    /// </summary>
    AFadeable FadeBehaviour { get; set; }

}
