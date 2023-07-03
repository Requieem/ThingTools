using UnityEngine;

[ExecuteInEditMode]
public class Displaceable : MonoBehaviour, IDisplaceable
{
    [SerializeField] ADisplaceable displaceable;

    Coroutine displace;
    Coroutine reset;

    public ADisplaceable DisplaceBehaviour { get { return displaceable; } set { displaceable = value; } }

    public void OnEnable()
    {
        if (displaceable == null) displaceable = new();
        Initialize(transform);
        DisplaceBehaviour.ClearDisplaced();
    }

    public void LateUpdate()
    {
        ShowDisplacement();

        if (!Application.isPlaying)
        {
            Initialize(transform);
        }
    }

    public void Start()
    {
        Initialize(transform);
        DisplaceBehaviour.ClearDisplaced();
    }

    public void Displace()
    {
        StopDisplacement();
        displace = StartCoroutine(DisplaceBehaviour.Displace());
    }

    public void ResetPosition()
    {
        StopDisplacement();
        reset = StartCoroutine(DisplaceBehaviour.ResetPosition());
    }

    public void ShowDisplacement()
    {
        DisplaceBehaviour.ShowDisplacement();
    }

    public void Initialize(Transform _transform)
    {
        DisplaceBehaviour.Initialize(transform);
    }

    void StopDisplacement()
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
}
