using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInLobbyDisplayManager : PlayerDetailsDisplayManager {

    private Dictionary<PlayerRef, string> _playerNames = new();
    private Dictionary<PlayerRef, bool> _playersReady = new();

    protected override void ResubscribeToEvents() {
        base.ResubscribeToEvents();

        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnNameUpdated += WhenPlayerNameUpdated;

        Player.OnPlayerToggledReady -= WhenPlayerToggledReady;
        Player.OnPlayerToggledReady += WhenPlayerToggledReady;
    }

    protected override void UnsubscribeFromEvents() {
        base.UnsubscribeFromEvents();

        Player.OnPlayerToggledReady -= WhenPlayerToggledReady;
    }

    private void WhenPlayerNameUpdated(PlayerRef playerRef, string newName) => UpdateName(playerRef, newName);

    private void WhenPlayerToggledReady(Player player, bool isReady) => UpdateReadyState(player, isReady);

    public new void AddEntry(PlayerRef playerRef, Player player) {
        base.AddEntry(playerRef, player);

        string name = string.Empty;
        bool isReady = false;

        _playerNames.Add(playerRef, name);
        _playersReady.Add(playerRef, isReady);
    }

    public new void RemoveEntry(PlayerRef playerRef) {
        base.RemoveEntry(playerRef);

        _playerNames.Remove(playerRef);
        _playersReady.Remove(playerRef);
    }

    public void UpdateName(PlayerRef playerRef, string newName) {
        if (!_playerEntries.TryGetValue(playerRef, out PlayerDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to update name of", this);
            return;
        }

        if (playerDisplay is PlayerLobbyDisplayUI == false) {
            Debug.LogWarning($"Player Display was not of type {nameof(PlayerLobbyDisplayUI)}", this);
            return;
        }

        PlayerLobbyDisplayUI playerUI = playerDisplay as PlayerLobbyDisplayUI;

        _playerNames[playerRef] = newName;
        playerUI.UpdateNameText(newName);
    }

    public void UpdateReadyState(Player player, bool isReady) {
        if (!_playerEntries.TryGetValue(player.PlayerID, out PlayerDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to update name of", this);
            return;
        }

        if (playerDisplay is PlayerLobbyDisplayUI == false) {
            Debug.LogWarning($"Player Display was not of type {nameof(PlayerLobbyDisplayUI)}", this);
            return;
        }

        PlayerLobbyDisplayUI playerUI = playerDisplay as PlayerLobbyDisplayUI;

        _playersReady[player.PlayerID] = isReady;
        playerUI.UpdateReadyText(isReady);
    }
}
