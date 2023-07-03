public interface IBehaviour<P>
{
    public abstract bool Activate(P _params);
    public abstract bool CanActivate();
}
