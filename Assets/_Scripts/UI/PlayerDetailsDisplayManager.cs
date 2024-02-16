using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetailsDisplayManager : MonoBehaviour {

    [SerializeField] protected PlayerDisplayUI[] _playerUIPanels;
    protected Dictionary<PlayerRef, PlayerDisplayUI> _playerEntries = new();

    protected virtual void OnEnable() {
        ResubscribeToEvents();
    }
    
    protected virtual void OnDisable() {
        UnsubscribeFromEvents();
    }

    protected virtual void ResubscribeToEvents() {
        Player.OnSpawned -= WhenPlayerSpawned;
        Player.OnSpawned += WhenPlayerSpawned;

        Player.OnDespawned -= WhenPlayerDespawns;
        Player.OnDespawned += WhenPlayerDespawns;
    }


    protected virtual void UnsubscribeFromEvents() {
        Player.OnSpawned -= WhenPlayerSpawned;
        Player.OnDespawned -= WhenPlayerDespawns;
    }

    protected virtual void WhenPlayerSpawned(Player player) => AddEntry(player);

    protected virtual void WhenPlayerDespawns(Player player) => RemoveEntry(player);

    protected virtual void AddEntry(Player player) {
        if (_playerEntries.ContainsKey(player.PlayerID)) {
            Debug.LogWarning($"Entries already contains Player Reference: {player.PlayerID}.", this);
            return;
        }

        if (player == null) {
            Debug.LogWarning($"Player is null", this);
            return;
        }

        PlayerDisplayUI display = GetPanelBasedOnPlayer(player);
        display.gameObject.SetActive(true);

        _playerEntries.Add(player.PlayerID, display);

        display.Initalize(player);
    }

    protected PlayerDisplayUI GetPanelBasedOnPlayer(Player player) {
        return player.IsHost ? _playerUIPanels[0] : _playerUIPanels[player.PlayerID + 1];
    }

    protected virtual void RemoveEntry(Player player) {
        if (!_playerEntries.TryGetValue(player.PlayerID, out PlayerDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to remove", this);
            return;
        }

        if (playerDisplay != null) {
            playerDisplay.gameObject.SetActive(false);
        }

        _playerEntries.Remove(player.PlayerID);
    }
}
