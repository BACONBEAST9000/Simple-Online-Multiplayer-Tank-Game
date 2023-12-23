using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreDisplayManager : PlayerDetailsDisplayManager {    
    private Dictionary<PlayerRef, string> _playerNames = new();
    private Dictionary<PlayerRef, int> _playerScores = new();

    protected override void ResubscribeToEvents() {
        base.ResubscribeToEvents();

        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnNameUpdated += WhenPlayerNameUpdated;

        Player.OnScoreUpdated -= WhenPlayerScoreUpdated;
        Player.OnScoreUpdated += WhenPlayerScoreUpdated;
    }

    protected override void UnsubscribeFromEvents() {
        base.UnsubscribeFromEvents();

        Player.OnScoreUpdated -= WhenPlayerScoreUpdated;
    }

    private void WhenPlayerNameUpdated(PlayerRef playerRef, string newName) => UpdateName(playerRef, newName);

    private void WhenPlayerScoreUpdated(PlayerRef playerRef, int newScore) => UpdateScore(playerRef, newScore);

    public new void AddEntry(PlayerRef playerRef, Player player) {
        base.AddEntry(playerRef, player);

        string name = string.Empty;
        int score = 0;

        _playerNames.Add(playerRef, name);
        _playerScores.Add(playerRef, score);
    }

    public new void RemoveEntry(PlayerRef playerRef) {
       base.RemoveEntry(playerRef);

        _playerNames.Remove(playerRef);
        _playerScores.Remove(playerRef);
    }

    public void UpdateName(PlayerRef playerRef, string newName) {
        print($"{playerRef} appearently updated their name in {nameof(PlayerScoreDisplayManager)}.");
        
        if(!_playerEntries.TryGetValue(playerRef, out PlayerDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to update name of", this);
            return;
        }

        if (playerDisplay is PlayerScoreDisplayUI == false) {
            Debug.LogWarning($"Player Display was not of type {nameof(PlayerScoreDisplayUI)}", this);
            return;
        }

        PlayerScoreDisplayUI playerScoreUI = playerDisplay as PlayerScoreDisplayUI;

        _playerNames[playerRef] = newName;
        playerScoreUI.UpdateNameText(newName);
    }
    
    public void UpdateScore(PlayerRef playerRef, int newScore) {
        if (!_playerEntries.TryGetValue(playerRef, out PlayerDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to update name of", this);
            return;
        }

        if (playerDisplay is PlayerScoreDisplayUI == false) {
            Debug.LogWarning($"Player Display was not of type {nameof(PlayerScoreDisplayUI)}", this);
            return;
        }

        PlayerScoreDisplayUI playerScoreUI = playerDisplay as PlayerScoreDisplayUI;

        _playerScores[playerRef] = newScore;
        playerScoreUI.UpdateScoreText(newScore);
    }
}
