using UnityEngine;

[ExecuteInEditMode]
public class Fadeable : MonoBehaviour, IFadeable
{
    [SerializeField] AFadeable fadeable = new();
    [SerializeField] bool fadeChildren = true;
    [SerializeField] bool fadeOnAwake = true;

    Coroutine[] fade;
    Coroutine[] reset;

    public AFadeable FadeBehaviour { get { return fadeable; } set { fadeable = value; } }
    public int FadeCount { get { return fadeChildren ? fadeable.Graphics.Length : 1; } }

    public void OnEnable()
    {
        Initialize(transform);
    }

    public void LateUpdate()
    {
        ShowFade();
    }

    public void Awake()
    {
        Initialize(transform);
        if (fadeOnAwake && Application.isPlaying)
        {
            Fade();
        }
    }

    public void Fade()
    {
        StopFade();

        fade = new Coroutine[FadeCount];

        for (int i = 0; i < FadeCount; i++)
        {
            fade[i] = StartCoroutine(fadeable.Fade(fadeable.Graphics[i], i));
        }
    }

    public void ResetFade()
    {
        StopFade();

        reset = new Coroutine[FadeCount];

        for (int i = 0; i < FadeCount; i++)
        {
            reset[i] = StartCoroutine(fadeable.ResetFade(fadeable.Graphics[i], i));
        }
    }

    public void ShowFade()
    {
        FadeBehaviour.ShowFade();
    }

    public void Initialize(Transform _transform)
    {
        FadeBehaviour.Initialize(transform);
    }

    void StopFade()
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
}
