using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [SerializeField] private List<Trait> defaultTraits = new List<Trait>();
    private List<PlayerData> _collectedData = new List<PlayerData>();
    private string _currentPlayerId = string.Empty;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string GetCurrentPlayerID()
    {
        return _currentPlayerId;
    }

    public void SetCurrentPlayerId(string playerId)
    {
        _currentPlayerId = playerId;
        // Ensure the player data exists when setting the ID
        AddOrUpdatePlayerData(_currentPlayerId, new Dictionary<string, int>());
    }

    public void AddOrUpdatePlayerData(string playerId, Dictionary<string, int> updatedScores)
    {
        var playerData = _collectedData.Find(p => p.PlayerId == playerId);
        if (playerData == null)
        {
            playerData = new PlayerData(playerId);
            _collectedData.Add(playerData);
        }

        foreach (var trait in defaultTraits)
        {
            if (updatedScores.TryGetValue(trait.Name, out int score))
            {
                playerData.UpdateTraitScore(trait.Name, score);
            }
        }
    }

    public void AddTraitScore(string traitName, int scoreChange)
    {
        var playerData = _collectedData.FirstOrDefault(p => p.PlayerId == _currentPlayerId);
        if (playerData != null)
        {
            if (playerData.TraitScores.ContainsKey(traitName))
            {
                playerData.TraitScores[traitName] += scoreChange;
            }
            else
            {
                playerData.TraitScores.Add(traitName, scoreChange);
            }
        }
    }

    public void ExportDataToCSV(string filePath)
    {
        StringBuilder sb = new StringBuilder();
        var header = "PlayerID,TimeStamp,";
        foreach (var trait in defaultTraits)
        {
            header += $"{trait.Name},";
        }
        sb.AppendLine(header.TrimEnd(','));

        foreach (var data in _collectedData)
        {
            var line = $"{data.PlayerId},{data.TimeStamp},";
            foreach (var trait in defaultTraits)
            {
                data.TraitScores.TryGetValue(trait.Name, out int score);
                line += $"{score},";
            }
            sb.AppendLine(line.TrimEnd(','));
        }

        File.WriteAllText(filePath, sb.ToString());
    }
}
