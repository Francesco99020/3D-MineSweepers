using System;

[Serializable]
public class HighScoreEntry
{
    public string difficulty;
    public int playerTime;

    public HighScoreEntry(string difficulty, int playerTime)
    {
        this.difficulty = difficulty;
        this.playerTime = playerTime;
    }
}
