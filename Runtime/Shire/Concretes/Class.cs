using System;
using UnityEngine;

/// <summary>
/// Represents a character class in the game.
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "Class", menuName = "ShireSoft/Class")]
public class Class : Shire<Class>
{
    [SerializeField]
    private Statistics statistics;

    /// <summary>
    /// Gets the statistics associated with the class.
    /// </summary>
    public Statistics Statistics { get { return statistics; } }

}