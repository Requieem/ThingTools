/// <summary>
/// Defines a builder that can generate instances of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the instance that this builder can build.</typeparam>
public interface IBuilder<T>
{
    /// <summary>
    /// Gets a copy of the built instance.
    /// </summary>
    /// <returns>A copy of the built instance of type <typeparamref name="T"/>.</returns>
    T GetCopy();

    /// <summary>
    /// Gets an instance of type <typeparamref name="T"/>. If no instance is currently built, a new one is built and stored.
    /// </summary>
    /// <returns>An instance of type <typeparamref name="T"/>.</returns>
    T GetInstance();

    /// <summary>
    /// Gets the built instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <value>The built instance of type <typeparamref name="T"/>.</value>
    T Built { get; }
}