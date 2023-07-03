public interface IDisplaceable : IInitializable
{
    public void ShowDisplacement();
    public void Displace();
    public void ResetPosition();
    public ADisplaceable DisplaceBehaviour { get; set; }
}
