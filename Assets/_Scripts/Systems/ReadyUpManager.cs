using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ReadyUpManager : NetworkBehaviour {

    public static event Action OnAllPlayersReady;

    private static List<Player> _readyPlayers = new();

    [SerializeField] private bool _allowSoloPlayForTesting;

    private void OnEnable() {
        PlayerReadyUp.OnPlayerToggledReady -= WhenPlayerTogglesReady;
        PlayerReadyUp.OnPlayerToggledReady += WhenPlayerTogglesReady;
    }

    private void OnDisable() {
        PlayerReadyUp.OnPlayerToggledReady -= WhenPlayerTogglesReady;
    }

    private void WhenPlayerTogglesReady(Player player, bool isPlayerReady) {
        if (isPlayerReady) {
            ReadyPlayer(player);
        }
        else {
            UnreadyPlayer(player);
        }
    }

    private bool _displayedReadyToPlay = false;
    public override void FixedUpdateNetwork() {
        if (!HasStateAuthority) return;
        
        if (ValadNumberOfPlayersReady()) {
            print("ALL PLAYERS ARE READY TO PLAY!");
            _displayedReadyToPlay = true;
            _readyPlayers.Clear();
            OnAllPlayersReady?.Invoke();
        }
    }

    private bool ValadNumberOfPlayersReady() {
        return !_displayedReadyToPlay && ValidNumberOfPlayers && AllPlayersReady;
    }

    private bool ValidNumberOfPlayers => _allowSoloPlayForTesting || _readyPlayers.Count > 1;
    private bool AllPlayersReady => _readyPlayers.Count > 0 && _readyPlayers.Count == PlayerManager.GetPlayerCount;

    public void ReadyPlayer(Player readyPlayer) {
        _readyPlayers.Add(readyPlayer);
    }

    public void UnreadyPlayer(Player notReadyPlayer) {
        _readyPlayers.Remove(notReadyPlayer);
    }
}
