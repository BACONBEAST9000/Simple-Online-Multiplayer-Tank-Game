using Fusion;
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour {

    public const float INVINCIBILITY_TIME = 3f;

    [SerializeField] private Player _player;
    [SerializeField] private NetworkTimer _networkTimer;

    [Networked] public TickTimer _invincibilityTimer { get; private set; }

    [Networked] public NetworkBool IsInvincible { get; set; }

    private void OnEnable() {
        _networkTimer.OnTimerEnd -= WhenInvincibilityTimerEnds;
        _networkTimer.OnTimerEnd += WhenInvincibilityTimerEnds;

        RespawnManager.OnRespawnedPlayer -= WhenPlayerRespawns;
        RespawnManager.OnRespawnedPlayer += WhenPlayerRespawns;
    }

    private void OnDisable() {
        RespawnManager.OnRespawnedPlayer -= WhenPlayerRespawns;
        RespawnManager.OnRespawnedPlayer -= WhenPlayerRespawns;
    }
    
    private void WhenPlayerRespawns(Player player) {
        if (player != _player) return;

        StartInvincibilityTimer();
    }
    
    private void WhenInvincibilityTimerEnds() {
        IsInvincible = false;
    }

    public void StartInvincibilityTimer() {
        IsInvincible = true;
        _networkTimer.StartTimer(INVINCIBILITY_TIME);
    }
}