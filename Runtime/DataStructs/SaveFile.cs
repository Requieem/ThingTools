using System;
using UnityEngine;
/// <summary>
/// Represents a save file containing information about the character and game progress.
/// </summary>
[Serializable]
public struct SaveFile
{
    /// <summary>
    /// The name of the save file.
    /// </summary>
    public string m_Name;

    /// <summary>
    /// The time when the save file was created.
    /// </summary>
    public DateTime m_Time;

    /// <summary>
    /// The total time played in the game.
    /// </summary>
    public double m_Time_played;

    /// <summary>
    /// The character data stored in the save file.
    /// </summary>
    public Character m_Character;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveFile"/> struct using the specified character and difficulty.
    /// </summary>
    /// <param name="character">The character to save.</param>
    /// <param name="difficulty">The difficulty level of the game.</param>
    public SaveFile(Character character, Difficulty difficulty)
    {
        m_Time_played = 0f;
        m_Time = DateTime.Now;
        m_Name = character.Name;
        this.m_Character = character;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveFile"/> struct by deserializing the provided JSON string.
    /// </summary>
    /// <param name="json">The JSON string representing the save file.</param>
    public SaveFile(string json)
    {
        var readFile = JsonUtility.FromJson<SaveFile>(json);
        m_Name = readFile.m_Name;
        m_Time = readFile.m_Time;
        m_Time_played = readFile.m_Time_played;
        m_Character = readFile.m_Character;
    }

    /// <summary>
    /// Serializes the save file to a JSON string.
    /// </summary>
    /// <returns>The JSON representation of the save file.</returns>
    public string Serialized()
    {
        return JsonUtility.ToJson(this);
    }

}