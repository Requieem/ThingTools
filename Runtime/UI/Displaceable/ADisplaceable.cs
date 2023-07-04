using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ADisplaceable
{
    #region Serialized Fields

    [SerializeField] private bool displace = false;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Vector3 displacement = Vector3.zero;
    [SerializeField] private Transform displacementContainer;
    [SerializeField, Range(0.1f, 5f)] private float displaceTime = 0.5f;

    #endregion

    #region Private Fields

    protected bool m_Displaced = false;
    private GameObject displacedCopy = null;
    private Transform transform = null;

    #endregion

    #region Properties

    public Vector3 DesiredPosition => RectTransform.parent.TransformPoint(originalPosition + displacement);
    public RectTransform RectTransform => transform as RectTransform;
    public RectTransform DisplacedTransform => displacedCopy.transform as RectTransform;
    public bool IsDisplacedCopy => m_Displaced;

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the displaceable object with the given Transform.
    /// </summary>
    /// <param name="_transform">The Transform to use for initialization.</param>
    public void Initialize(Transform _transform)
    {
        transform = _transform;
        originalPosition = RectTransform.localPosition;
    }

    /// <summary>
    /// Show the displacement of the object if conditions are met.
    /// </summary>
    public void ShowDisplacement()
    {
        if (Application.isPlaying && m_Displaced)
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

        if (!Application.isPlaying && !m_Displaced && displace && displacedCopy == null)
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

    /// <summary>
    /// Clears the displaced copy object.
    /// </summary>
    public void ClearDisplaced()
    {
        if (displacedCopy != null)
        {
            ConditionalDestroy(displacedCopy);
        }

        displacedCopy = null;
    }

    /// <summary>
    /// Coroutine that smoothly slides a given RectTransform to a given position.
    /// </summary>
    /// <returns>Coroutine IEnumerator</returns>
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

    /// <summary>
    /// Coroutine that smoothly slides a given RectTransform back to its original position.
    /// </summary>
    /// <returns>Coroutine IEnumerator</returns>
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

    #endregion

    #region Private Methods

    /// <summary>
    /// Destroys the GameObject based on the application's state.
    /// </summary>
    /// <param name="gameObject">GameObject to be destroyed.</param>
    private void ConditionalDestroy(GameObject gameObject)
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

    #endregion

}