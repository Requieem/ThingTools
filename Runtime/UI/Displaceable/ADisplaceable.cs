using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[Serializable]
public class ADisplaceable
{
    [SerializeField] bool displace = false;
    [SerializeField] Vector3 displacement = Vector3.zero;
    [SerializeField][Range(0.1f, 5f)] float displaceTime = 0.5f;
    [SerializeField] Transform displacementContainer;

    protected bool displaced = false;
    [SerializeField]
    Vector3 originalPosition;

    GameObject displacedCopy = null;
    Transform transform = null;

    public Vector3 DesiredPosition { get { return RectTransform.parent.TransformPoint(originalPosition + displacement); } }
    public RectTransform RectTransform { get { return transform as RectTransform; } }
    public RectTransform DisplacedTransform { get { return displacedCopy.transform as RectTransform; } }
    public bool IsDisplacedCopy { get { return displaced; } }

    public void Initialize(Transform _transform)
    {
        transform = _transform;
        originalPosition = RectTransform.localPosition;
    }

    public void ConditionalDestroy(GameObject gameObject)
    {
        if (Application.isPlaying)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.DestroyImmediate(gameObject);
        }
    }

    public void ShowDisplacement()
    {
        if (Application.isPlaying && displaced)
        {
            ConditionalDestroy(transform.gameObject);
            return;
        }

        if (!displace && displacedCopy != null)
        {
            ConditionalDestroy(displacedCopy);
            displacedCopy = null;
            return;
        }

        if (!Application.isPlaying && !displaced && displace && displacedCopy == null)
        {
            displacedCopy = GameObject.Instantiate(transform.gameObject, displacementContainer, true);

            // remove the displaceable component from the copy
            var displaceableCopy = displacedCopy.GetComponents<Displaceable>();
            foreach (var displaceable in displaceableCopy)
            {
                displaceable.enabled = false;
            }

            DisplacedTransform.position = DesiredPosition;

            var images = displacedCopy.GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.25f);
            }
            var texts = displacedCopy.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in texts)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.25f);
            }
        }
        else if (displace && displacedCopy != null && transform.parent.InverseTransformPoint(DisplacedTransform.position) != DesiredPosition)
        {
            // update the displacement to be equal to the distance vector between this transform and the displaced transform
            var difference = transform.parent.InverseTransformPoint(DisplacedTransform.position) - RectTransform.localPosition;
            if (!(difference == displacement))
            {
                displacement = difference;
            }
        }

        if (displacedCopy != null)
        {
            // draw a line that goes from this object to the displaced Copy
            Debug.DrawLine(RectTransform.position, DisplacedTransform.position, Color.yellow);
        }
    }

    public void ClearDisplaced()
    {
        if (displacedCopy != null)
        {
            ConditionalDestroy(displacedCopy);
        }

        displacedCopy = null;
    }

    // coroutine that smoothly slides a given rect transform to a given position
    public IEnumerator Displace()
    {
        var elapsedTime = 0f;
        Vector3 startingPos = RectTransform.position;
        Vector3 desiredPos = DesiredPosition;

        while (elapsedTime < displaceTime)
        {
            yield return new WaitForEndOfFrame();
            RectTransform.position = Vector3.Lerp(startingPos, desiredPos, (elapsedTime / displaceTime));
            elapsedTime += Time.deltaTime;
        }

        elapsedTime = displaceTime;
        RectTransform.position = desiredPos;
    }

    // coroutine that smoothly slides a given rect transform to a given position
    public IEnumerator ResetPosition()
    {
        var elapsedTime = 0f;
        Vector3 startingPos = RectTransform.localPosition;

        while (elapsedTime < displaceTime)
        {
            yield return new WaitForEndOfFrame();
            RectTransform.localPosition = Vector3.Lerp(startingPos, originalPosition, (elapsedTime / displaceTime));
            elapsedTime += Time.deltaTime;
        }

        RectTransform.localPosition = originalPosition;
    }
}
