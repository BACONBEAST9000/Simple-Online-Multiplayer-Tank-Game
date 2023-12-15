using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplayManager : NetworkBehaviour {

    [SerializeField] private RectTransform _parentUIObject;
    [SerializeField] private PlayerScoreDisplayUI _scoreDisplayPrefab;

    private Dictionary<PlayerRef, PlayerScoreDisplayUI> _playerEntries = new();
    
    private Dictionary<PlayerRef, string> _playerNames = new();
    private Dictionary<PlayerRef, int> _playerScores = new();

    private void OnEnable() {
        Player.OnSpawned -= WhenPlayerSpawned;
        Player.OnSpawned += WhenPlayerSpawned;

        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnNameUpdated += WhenPlayerNameUpdated;

        Player.OnScoreUpdated -= WhenPlayerScoreUpdated;
        Player.OnScoreUpdated += WhenPlayerScoreUpdated;

        Player.OnDespawned -= WhenPlayerDespawns;
        Player.OnDespawned += WhenPlayerDespawns;
    }


    private void OnDisable() {
        Player.OnSpawned -= WhenPlayerSpawned;
        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnScoreUpdated -= WhenPlayerScoreUpdated;
        Player.OnDespawned -= WhenPlayerDespawns;
    }

    private void WhenPlayerSpawned(PlayerRef playerRef, Player player) => AddEntry(playerRef, player);

    private void WhenPlayerDespawns(PlayerRef playerRef, Player player) => RemoveEntry(playerRef);

    private void WhenPlayerNameUpdated(PlayerRef playerRef, string newName) => UpdateName(playerRef, newName);

    private void WhenPlayerScoreUpdated(PlayerRef playerRef, int newScore) => UpdateScore(playerRef, newScore);

    public void AddEntry(PlayerRef playerRef, Player player) {
        if (_playerEntries.ContainsKey(playerRef)) {
            Debug.LogWarning($"Entries already contains Player Reference: {playerRef}.", this);
            return;
        }

        if (player == null) {
            Debug.LogWarning($"Player is null", this);
            return;
        }

        PlayerScoreDisplayUI scoreDisplay = Instantiate(_scoreDisplayPrefab, transform);

        string name = string.Empty;
        int score = 0;

        _playerNames.Add(playerRef, name);
        _playerScores.Add(playerRef, score);
        _playerEntries.Add(playerRef, scoreDisplay);

        scoreDisplay.UpdateEntry(player);
    }

    public void RemoveEntry(PlayerRef playerRef) {
        if(!_playerEntries.TryGetValue(playerRef, out PlayerScoreDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to remove", this);
            return;
        }

        if (playerDisplay != null) {
            Destroy(playerDisplay.gameObject);
        }

        _playerNames.Remove(playerRef);
        _playerScores.Remove(playerRef);

        _playerEntries.Remove(playerRef);
    }

    public void UpdateName(PlayerRef playerRef, string newName) {
        if(!_playerEntries.TryGetValue(playerRef, out PlayerScoreDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to update name of", this);
            return;
        }

        _playerNames[playerRef] = newName;
        playerDisplay.UpdateNameText(newName);
    }
    
    public void UpdateScore(PlayerRef playerRef, int newScore) {
        if(!_playerEntries.TryGetValue(playerRef, out PlayerScoreDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to update score of", this);
            return;
        }

        _playerScores[playerRef] = newScore;
        playerDisplay.UpdateScoreText(newScore);
    }
}
