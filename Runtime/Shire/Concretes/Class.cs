using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Class", menuName = "ShireSoft/Class")]
public class Class : Shire<Class>
{
    [SerializeField]
    Statistics statistics;

    public Statistics Statistics { get { return statistics; } }
}
