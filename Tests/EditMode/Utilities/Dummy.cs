public class Dummy
{

    public float value = 0f;

    public int CompareTo(Dummy other)
    {
        return value.CompareTo(other.value);
    }

    public static bool operator ==(Dummy x, Dummy y)
    {
        return x.value == y.value;
    }

    public static bool operator !=(Dummy x, Dummy y)
    {
        return x.value != y.value;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
