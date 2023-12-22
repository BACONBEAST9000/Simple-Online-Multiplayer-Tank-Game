using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetailsDisplayManager : MonoBehaviour {

    [SerializeField] protected RectTransform _parentUIObject;
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

    protected virtual void WhenPlayerSpawned(PlayerRef playerRef, Player player) => AddEntry(playerRef, player);

    protected virtual void WhenPlayerDespawns(PlayerRef playerRef, Player player) => RemoveEntry(playerRef);

    protected virtual void AddEntry(PlayerRef playerRef, Player player) {
        if (_playerEntries.ContainsKey(playerRef)) {
            Debug.LogWarning($"Entries already contains Player Reference: {playerRef}.", this);
            return;
        }

        if (player == null) {
            Debug.LogWarning($"Player is null", this);
            return;
        }

        PlayerDisplayUI display = Instantiate(_playerDisplayPrefab, transform);

        _playerEntries.Add(playerRef, display);

        display.UpdateEntry(player);
    }

    protected virtual void RemoveEntry(PlayerRef playerRef) {
        if (!_playerEntries.TryGetValue(playerRef, out PlayerDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to remove", this);
            return;
        }

        if (playerDisplay != null) {
            Destroy(playerDisplay.gameObject);
        }

        _playerEntries.Remove(playerRef);
    }
}