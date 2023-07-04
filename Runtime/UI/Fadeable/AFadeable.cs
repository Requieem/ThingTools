using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AFadeable
{
    [SerializeField] private bool fade = false;
    [SerializeField] private float targetAlpha = 0f;
    [SerializeField, Range(0.1f, 5f)] private float fadeTime = 0.5f;

    private Transform transform;
    private List<float> originalAlphas;
    private bool isFaded = false;

    /// <summary>
    /// The RectTransform of the fadeable object.
    /// </summary>
    public RectTransform RectTransform { get { return transform as RectTransform; } }

    /// <summary>
    /// The Graphics components of the fadeable object.
    /// </summary>
    public Graphic[] Graphics { get { return transform.GetComponentsInChildren<Graphic>(); } }

    /// <summary>
    /// The target object that implements the IFadeable interface.
    /// </summary>
    public IFadeable Target { get { return transform.GetComponent<IFadeable>(); } }

    /// <summary>
    /// Initializes the fadeable object with the given Transform.
    /// </summary>
    /// <param name="_transform">The Transform to use for initialization.</param>
    public virtual void Initialize(Transform _transform)
    {
        transform = _transform;
        originalAlphas = new List<float>();

        foreach (var graphic in Graphics)
        {
            originalAlphas.Add(graphic.color.a);
        }
    }

    /// <summary>
    /// Shows the fade effect based on the fade state.
    /// </summary>
    public void ShowFade()
    {
        if (fade && !isFaded)
        {
            isFaded = true;
            Target.Fade();
        }
        else if (!fade && isFaded)
        {
            isFaded = false;
            Target.ResetFade();
        }
    }

    /// <summary>
    /// Coroutine that fades a graphic component to the target alpha value.
    /// </summary>
    /// <param name="graphic">The Graphic component to fade.</param>
    /// <param name="graphicIndex">The index of the graphic component in the list of Graphics.</param>
    public IEnumerator Fade(Graphic graphic, int graphicIndex)
    {
        float elapsedTime = 0;
        float startingAlpha = graphic.color.a;
        float desiredAlpha = targetAlpha;
        Color color = graphic.color;

        while (elapsedTime < fadeTime)
        {
            graphic.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startingAlpha, desiredAlpha, elapsedTime / fadeTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        graphic.color = new Color(color.r, color.g, color.b, desiredAlpha);
    }

    /// <summary>
    /// Coroutine that resets the fade of a graphic component to its original alpha value.
    /// </summary>
    /// <param name="graphic">The Graphic component to reset.</param>
    /// <param name="graphicIndex">The index of the graphic component in the list of Graphics.</param>
    public IEnumerator ResetFade(Graphic graphic, int graphicIndex)
    {
        if (graphicIndex < 0 || graphicIndex >= originalAlphas.Count)
            yield break; // Invalid index

        float elapsedTime = 0;
        float startingAlpha = graphic.color.a;
        float desiredAlpha = originalAlphas[graphicIndex];
        Color color = graphic.color;

        while (elapsedTime < fadeTime)
        {
            graphic.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startingAlpha, desiredAlpha, elapsedTime / fadeTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        graphic.color = new Color(color.r, color.g, color.b, desiredAlpha);
    }
}