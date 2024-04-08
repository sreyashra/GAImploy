using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public string PlayerId;
    public Dictionary<string, int> TraitScores = new Dictionary<string, int>();
    public long TimeStamp;

    public PlayerData(string playerId)
    {
        PlayerId = playerId;
        TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public void UpdateTraitScore(string traitName, int score)
    {
        if (TraitScores.ContainsKey(traitName))
        {
            TraitScores[traitName] = score;
        }
        else
        {
            TraitScores.Add(traitName, score);
        }
    }
}

[System.Serializable]
public class Trait
{
    public string Name;
    public int Score;
}