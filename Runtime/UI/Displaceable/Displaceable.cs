using UnityEngine;

/// <summary>
/// Displaceable class that enables an object to be displaced.
/// This class must be attached to a GameObject in the scene.
/// </summary>
[ExecuteInEditMode]
public class Displaceable : MonoBehaviour, IDisplaceable
{
    #region Serialized Fields

    /// <summary>
    /// The displaceable object that this script operates on.
    /// </summary>
    [SerializeField] private ADisplaceable displaceable;

    #endregion

    #region Private Fields

    /// <summary>
    /// Coroutine responsible for displacement.
    /// </summary>
    private Coroutine displace;

    /// <summary>
    /// Coroutine responsible for resetting position.
    /// </summary>
    private Coroutine reset;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the displaceable behaviour of the object.
    /// </summary>
    public ADisplaceable DisplaceBehaviour
    {
        get { return displaceable; }
        set { displaceable = value; }
    }

    #endregion

    #region Unity Callbacks

    /// <summary>
    /// Unity callback that runs when the script instance is being loaded.
    /// Initializes the displaceable object.
    /// </summary>
    private void OnEnable()
    {
        if (displaceable == null) displaceable = new ADisplaceable();
        Initialize(transform);
        DisplaceBehaviour.ClearDisplaced();
    }

    /// <summary>
    /// Unity callback that runs every frame after all Update functions have been called.
    /// Shows displacement and reinitializes the displaceable object if the application is not playing.
    /// </summary>
    private void LateUpdate()
    {
        PreviewDisplacement();

        if (!Application.isPlaying)
        {
            Initialize(transform);
        }
    }

    /// <summary>
    /// Unity callback that runs on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// Initializes the displaceable object and clears the displaced copy object.
    /// </summary>
    private void Start()
    {
        Initialize(transform);
        DisplaceBehaviour.ClearDisplaced();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts the displacement of the object.
    /// </summary>
    public void Displace()
    {
        StopDisplacement();
        displace = StartCoroutine(DisplaceBehaviour.Displace());
    }

    /// <summary>
    /// Resets the position of the object.
    /// </summary>
    public void ResetPosition()
    {
        StopDisplacement();
        reset = StartCoroutine(DisplaceBehaviour.ResetPosition());
    }

    /// <summary>
    /// Shows the displacement of the object.
    /// </summary>
    public void PreviewDisplacement()
    {
        DisplaceBehaviour.ShowDisplacement();
    }

    /// <summary>
    /// Initializes the displaceable object with the given Transform.
    /// </summary>
    /// <param name="_transform">The Transform to use for initialization.</param>
    public void Initialize(Transform _transform)
    {
        DisplaceBehaviour.Initialize(transform);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Stops the displacement and reset coroutines.
    /// </summary>
    private void StopDisplacement()
    {
        if (displace != null)
        {
            StopCoroutine(displace);
            displace = null;
        }

        if (reset != null)
        {
            StopCoroutine(reset);
            reset = null;
        }
    }

    #endregion
}
