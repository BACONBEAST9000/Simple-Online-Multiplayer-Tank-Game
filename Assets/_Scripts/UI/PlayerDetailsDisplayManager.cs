using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetailsDisplayManager : MonoBehaviour {

    [SerializeField] protected PlayerDisplayUI _playerDisplayPrefab;

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

        PlayerDisplayUI display = Instantiate(_playerDisplayPrefab, transform);

        _playerEntries.Add(player.PlayerID, display);

        display.UpdateEntry(player);
    }

    protected virtual void RemoveEntry(Player player) {
        if (!_playerEntries.TryGetValue(player.PlayerID, out PlayerDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to remove", this);
            return;
        }

        if (playerDisplay != null) {
            Destroy(playerDisplay.gameObject);
        }

        _playerEntries.Remove(player.PlayerID);
    }
}
