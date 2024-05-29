using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    List<HighScoreEntry> entries = new List<HighScoreEntry>();

    public void AddUserToList(string difficulty, int playerTime)
    {
        entries.Add(new HighScoreEntry(difficulty, playerTime));
    }
}

