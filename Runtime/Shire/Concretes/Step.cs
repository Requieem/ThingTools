using System;
using UnityEngine;

[Serializable]
public class Step<T> : Shire<T> where T : Step<T>
{
    #region Serialized Fields
    [SerializeField] private bool m_IsMinLevel = false;
    [SerializeField] private bool m_IsMaxLevel = false;
    [SerializeField] private float m_ExpToThisLevel;
    [SerializeField] private float m_ExpToNextLevel;
    [SerializeField] private int m_LevelPoints;

    [SerializeField] private T m_NextLevel;
    [SerializeField] private T m_PreviousLevel;

    #endregion

    #region Properties

    public bool IsMaxLevel => m_IsMaxLevel;
    public bool IsMinLevel => m_IsMinLevel;
    public float ExpToThisLevel => m_ExpToThisLevel;
    public float ExpToNextLevel => m_ExpToNextLevel;
    public float RequiredExp => ExpToNextLevel - ExpToThisLevel;
    public int LevelPoints => m_LevelPoints;
    public T PreviousLevel => m_PreviousLevel;
    public T NextLevel => m_NextLevel;

    #endregion

    #region Public Methods

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
        return m_ExpToNextLevel + m_ExpToThisLevel;
    }

    public void Sync()
    {
        FindSiblings();
        ComputeExpForNext();
    }

    #endregion

    #region Overrides

    public override void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();
    }

    public override void OnAfterDeserialize()
    {
        base.OnAfterDeserialize();
    }

    #endregion

}