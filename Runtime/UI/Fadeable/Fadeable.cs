using UnityEngine;

/// <summary>
/// Component that provides fading functionality to a GameObject and its child Graphic components.
/// </summary>
[ExecuteInEditMode]
public class Fadeable : MonoBehaviour, IFadeable
{
    #region Serialized Fields

    [SerializeField]
    private AFadeable fadeable = new AFadeable();

    [SerializeField]
    private bool fadeChildren = true;

    [SerializeField]
    private bool fadeOnAwake = true;

    #endregion

    #region Private Fields

    private Coroutine[] fade;
    private Coroutine[] reset;

    #endregion

    #region Properties

    /// <summary>
    /// The fade behavior associated with the Fadeable component.
    /// </summary>
    public AFadeable FadeBehaviour
    {
        get { return fadeable; }
        set { fadeable = value; }
    }

    /// <summary>
    /// The number of Graphic components to apply fade effects to.
    /// </summary>
    public int FadeCount => fadeChildren ? fadeable.Graphics.Length : 1;

    #endregion

    #region Unity Callbacks

    private void OnEnable()
    {
        Initialize(transform);
    }

    private void LateUpdate()
    {
        PreviewFade();
    }

    private void Awake()
    {
        Initialize(transform);
        if (fadeOnAwake && Application.isPlaying)
        {
            Fade();
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initiates the fading effect on the GameObject and its child Graphic components.
    /// </summary>
    public void Fade()
    {
        StopFade();

        fade = new Coroutine[FadeCount];

        for (int i = 0; i < FadeCount; i++)
        {
            fade[i] = StartCoroutine(fadeable.Fade(fadeable.Graphics[i], i));
        }
    }

    /// <summary>
    /// Resets the fading effect on the GameObject and its child Graphic components.
    /// </summary>
    public void ResetFade()
    {
        StopFade();

        reset = new Coroutine[FadeCount];

        for (int i = 0; i < FadeCount; i++)
        {
            reset[i] = StartCoroutine(fadeable.ResetFade(fadeable.Graphics[i], i));
        }
    }

    /// <summary>
    /// Shows the fading effect on the GameObject and its child Graphic components.
    /// </summary>
    public void PreviewFade()
    {
        FadeBehaviour.ShowFade();
    }

    /// <summary>
    /// Initializes the Fadeable component with the specified transform.
    /// </summary>
    /// <param name="_transform">The transform to use for initialization.</param>
    public void Initialize(Transform _transform)
    {
        FadeBehaviour.Initialize(transform);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Stops all active fading coroutines.
    /// </summary>
    private void StopFade()
    {
        if (fade != null)
        {
            foreach (var f in fade)
            {
                if (f != null)
                {
                    StopCoroutine(f);
                }
            }
            fade = null;
        }

        if (reset != null)
        {
            foreach (var r in reset)
            {
                if (r != null)
                {
                    StopCoroutine(r);
                }
            }
            reset = null;
        }
    }

    #endregion
}
