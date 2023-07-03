using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AFadeable
{
    [SerializeField] bool fade = false;
    [SerializeField] float targetAlpha = 0f;
    [SerializeField][Range(0.1f, 5f)] float fadeTime = 0.5f;

    Transform transform;

    List<float> originalAlphas;
    bool isFaded = false;

    public RectTransform RectTransform { get { return transform as RectTransform; } }
    public Graphic[] Graphics { get { return transform.GetComponentsInChildren<Graphic>(); } }
    public IFadeable Target { get { return transform.GetComponent<IFadeable>(); } }

    public virtual void Initialize(Transform _transform)
    {
        transform = _transform;
        originalAlphas = new List<float>();

        foreach (var graphic in Graphics)
        {
            originalAlphas.Add(graphic.color.a);
        }
    }

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

    public IEnumerator ResetFade(Graphic graphic, int graphicIndex)
    {
        if (graphicIndex < 0 || graphicIndex >= originalAlphas.Count) yield break; // invalid index

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
