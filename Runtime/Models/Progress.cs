using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// An abstract base class for all lower-level Progresses that exist in the game.
/// </summary>
[Serializable]
public class Progress : ObjectContainer<float>
{
    #region Instance Fields:

    [SerializeField]
    List<Requirement> requirements = new();
    [SerializeField]
    List<Requirement> satisfied = new();

    [SerializeField]
    float currentProgress = 0f;
    [SerializeField]
    bool linearProgression = true;

    protected DataTrigger<float> m_OnProgress;
    protected DataTrigger<float> m_OnRegress;
    protected UnityEvent m_OnReached;

    public float CurrentProgress
    {
        get
        {
            if (requirements.Count == 0)
                return 1;
            currentProgress = satisfied.Count / requirements.Count;
            return currentProgress;
        }
        set
        {
            if (currentProgress != value)
            {
                currentProgress = value;
            }
        }
    }
    public float NeededProgress { get { return 1 - CurrentProgress; } }

    #endregion

    #region Instance Properties:

    /// <summary>
    /// Gets a value indicating whether the Progress is met.
    /// </summary>
    public virtual bool IsFinished { get { return currentProgress == requirements.Count; } }

    /// <summary>
    /// Gets the Unity event that is invoked when some progress happens.
    /// </summary>
    public virtual DataTrigger<float> OnProgress { get { return m_OnProgress; } }

    /// <summary>
    /// Gets the Unity event that is invoked when the progress regresses.
    /// </summary>
    public virtual DataTrigger<float> OnRegress { get { return m_OnRegress; } }

    /// <summary>
    /// Gets the Unity event that is invoked when the Progress is met.
    /// </summary>
    public virtual UnityEvent OnReached { get { return m_OnReached; } }

    public override Comparison<float> Comparer { get { return (a, b) => a.CompareTo(b); } }

    public int m_ReachedIndex = 0;

    #endregion

    #region Initializers:

    /// <summary>
    /// Initializes the Progress with the specified parameters.
    /// </summary>
    /// <param name="requirements">A list of requirements that must be met.</param>
    /// <param name="onProgress">AEvent that is invoked when some progress happens.</param>
    /// <param name="onRegress">AEvent that is invoked when the progress regresses.</param>
    /// <param name="onReached">AEvent that is invoked when the Progress is met.</param>
    public Progress(List<Requirement> requirements, DataTrigger<float> onProgress, DataTrigger<float> onRegress, UnityEvent onReached)
    {
        this.requirements = requirements;
        this.m_OnProgress = onProgress;
        this.m_OnRegress = onRegress;
        this.m_OnReached = onReached;
    }

    #endregion

    #region Instance Methods:

    public override void OnEnable()
    {
        base.OnEnable();
        CheckProgress();
    }

    /// <summary>
    /// A function that checks if the requirements are met, and sets the currentProgress accordingly.
    /// </summary>
    /// <remarks> 
    /// overly complicated, could just iterate over the list with a for loop instead.
    /// </remarks>
    public virtual void CheckProgress()
    {
        satisfied.Clear();
        m_ReachedIndex = 0;
        CurrentProgress = 0;
        requirements = requirements.Where(r => r != null).ToList();
        // go over the requirements list and push satisfied requirements to the satisfied list
        for(int i = 0; i < requirements.Count; i++)
        {
            if (requirements[i] == null)
                continue;
            if (requirements[i].IsSatisfied)
            {
                if(linearProgression && i - m_ReachedIndex > 1)
                {
                    break;
                }
                satisfied.Add(requirements[i]);
                requirements[i].OnUnsatisfied.AddListener(Regress);
                m_ReachedIndex = i;
            }
            else
            {
                requirements[i].OnSatisfied.AddListener(DoProgress);
            }

        }

        if(IsWatching(CurrentProgress))
        {
            Satisfier.Satisfy(CurrentProgress);
        }
    }

    /// <summary>
    /// A function that progresses through the requirements.
    /// </summary>
    public virtual void DoProgress(Requirement requirement)
    {
        var index = requirements.IndexOf(requirement);
        if (index == -1 || (linearProgression && !ProgressCondition(index)))
            return;

        while (index < requirements.Count && requirements[index].IsSatisfied)
        {
            MoreProgress(index++);
        }

        m_ReachedIndex = index - 1;

    }
    void MoreProgress(int index)
    {
        satisfied.Add(requirements[index]);
        satisfied.Last().OnUnsatisfied.AddListener(Regress);
        CheckSatisfyOnChange(CurrentProgress);
    }

    /// <summary>
    /// A function that regresses through the requirements.
    /// </summary>
    public virtual void Regress(Requirement requirement)
    {
        var index = satisfied.IndexOf(requirement);
        if (index == -1)
            return;

        m_ReachedIndex = index - 1;
        if(linearProgression)
        {
            /* remove all requirement from the satisfied list from index to count - 1 */
            while (index < satisfied.Count)
            {
                LessProgress(index++);
            }
        }
        else
        {
            LessProgress(index);
        }

    }
    void LessProgress(int index)
    {
        satisfied[index].OnSatisfied.AddListener(DoProgress);
        satisfied.RemoveAt(index);
        CheckUnsatisfyOnChange(CurrentProgress);
    }

    public bool ProgressCondition(int index)
    {
        return index - m_ReachedIndex <= 1;
    }

    public override float WatchFeedback(float obj, UnityEvent onSatisfy, UnityEvent onUnsatisfy)
    {
        Satisfier.Watch(obj, onSatisfy, onUnsatisfy);
        return CurrentProgress;
    }

    #endregion
}
