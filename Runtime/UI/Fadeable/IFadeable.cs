public interface IFadeable : IInitializable
{
    public void ShowFade();
    public void Fade();
    public void ResetFade();
    public AFadeable FadeBehaviour { get; set; }
}
