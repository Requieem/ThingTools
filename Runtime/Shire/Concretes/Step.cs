using System;

[Serializable]
public class Step<T> : Shire<T> where T : Step<T>
{
    public bool m_IsMinLevel = false;
    public bool m_IsMaxLevel = false;
    public float m_ExpToThisLevel;
    public float m_ExpToNextLevel;
    public int m_LevelPoints;

    public T m_NextLevel;
    public T m_PreviousLevel;

    public bool IsMaxLevel { get { return m_IsMaxLevel; } }
    public bool IsMinLevel { get { return m_IsMinLevel; } }
    public float RequiredExp { get { return m_ExpToNextLevel - m_ExpToThisLevel; } }

    public override void Enable()
    {
        base.Enable();
        Sync();
        Lister.OnAdd += Sync;
        Lister.OnRemove += Sync;
    }

    private void OnDisable()
    {
        Lister.OnAdd -= Sync;
        Lister.OnRemove -= Sync;
    }

    public bool Reached(Step<T> level)
    {
        return m_ExpToThisLevel >= level.m_ExpToThisLevel;
    }

    public override void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();
    }

    public void FindSiblings()
    {
        var isPreviousValid = Lister.Siblings.Count > Order && Order > 0;
        m_PreviousLevel = isPreviousValid ? Lister.Siblings[Order - 1] : null;

        var isNextValid = Lister.Siblings.Count > Order + 1;
        m_NextLevel = isNextValid ? Lister.Siblings[Order + 1] : null;

        if (IsMinLevel || Order == 0)
        {
            m_Order = 0;
            m_IsMinLevel = true;
            m_PreviousLevel = null;
        }
        else if (isPreviousValid)
        {
            m_PreviousLevel.m_NextLevel = this as T;
        }

        if (IsMaxLevel)
        {
            m_NextLevel = null;
            m_IsMaxLevel = true;
        }
        else if (isNextValid)
        {
            m_NextLevel.m_PreviousLevel = this as T;
        }
    }

    public float ComputeExpForNext()
    {
        /*        m_ExpToThisLevel = m_PreviousLevel?.ComputeExpForNext() ?? 0;*/
        return m_ExpToNextLevel + m_ExpToThisLevel;
    }

    public void Sync()
    {
        FindSiblings();
        ComputeExpForNext();
    }

    public override void OnAfterDeserialize()
    {
        base.OnAfterDeserialize();
    }
}
