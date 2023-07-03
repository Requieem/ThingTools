using System;
using UnityEngine;

[Serializable]
public struct SaveFile
{
    public string name;
    public DateTime time;
    public double time_played;
    public Character character;

    public SaveFile(Character character, Difficulty difficulty)
    {
        time_played = 0f;
        time = DateTime.Now;
        name = character.Name;
        this.character = character;
    }

    public SaveFile(string json)
    {
        var _readFile = JsonUtility.FromJson<SaveFile>(json);
        name = _readFile.name;
        time = _readFile.time;
        time_played = _readFile.time_played;
        character = _readFile.character;
    }

    public string Serialized() { return JsonUtility.ToJson(this); }
}
