using Fusion;
using System;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Player : NetworkBehaviour, IDamageable {
    public const float RESPAWN_DELAY_SECONDS = 2;

    public static event Action<Player> OnPlayerDestroyed;
    public static event Action<Player> OnPlayerRespawned;
    public static event Action<Player, bool> OnPlayerToggledReady;

    public static event Action<Player, int> OnScoreUpdated;
    public static event Action<Player, string> OnNameUpdated;
    public static event Action<Player> OnSpawned;
    public static event Action<Player> OnDespawned;

    [Networked(OnChanged = nameof(OnScoreChanged))]
    [HideInInspector]
    public int Score { get; private set; }

    [HideInInspector]
    [Networked(OnChanged = nameof(OnNameChanged))]
    public NetworkString<_16> NickName { get; private set; }

    [Networked] private TickTimer _respawnTimer { get; set; }

    [Networked(OnChanged = nameof(OnPlayerAliveChanged))]
    [HideInInspector]
    public NetworkBool IsAlive { get; private set; } = true;

    [SerializeField] private Collider _collider;

    // TODO: Rework this
    [SerializeField] private PlayerVisuals _playerVisuals;
    public PlayerVisuals GetPlayerVisuals => _playerVisuals;

    [Networked(OnChanged = nameof(OnPlayerReadyChanged))]
    [HideInInspector]
    public NetworkBool IsReady { get; private set; }

    [Networked] private NetworkButtons _previousButtons { get; set; }

    [SerializeField] private PlayerInvincibility _playerInvincibility;

    public int PlayerID { get; private set; }

    public bool IsHost => PlayerID == Runner.SessionInfo.MaxPlayers - 1;

    public override void Spawned() {
        IsAlive = true;
        IsReady = false;
        PlayerID = Object.InputAuthority;

        if (Object.HasInputAuthority) {
            var name = FindObjectOfType<PlayerData>().NickName;
            RpcSetNickName(name);
        }

        OnSpawned?.Invoke(this);

        PlayerManager.AddPlayer(this);
    }

    public override void Despawned(NetworkRunner runner, bool hasState) {
        OnDespawned?.Invoke(this);
        PlayerManager.RemovePlayer(this);
    }

    public override void FixedUpdateNetwork() {
        if (_respawnTimer.Expired(Runner)) {
            IsAlive = true;
            _collider.enabled = true;
            _respawnTimer = default;
            RespawnManager.Instance.Respawn(this);
        }

        if (GetInput(out PlayerInput input)) {
            if (IsReadyButtonPressed(input)) {
                IsReady = !IsReady;
            }
        }
    }

    private bool IsReadyButtonPressed(PlayerInput input) {
        return input.Buttons.WasPressed(_previousButtons, ActionButtons.Ready);
    }

    // RPC used to send player information to the Host
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void RpcSetNickName(string nickName) {
        if (string.IsNullOrEmpty(nickName)) return;
        NickName = nickName;
    }

    public void IncrementScoreBy(int value) => Score += value;
    public void DecrementScoreBy(int value) => Score = Mathf.Max(0, Score - value);

    public static void OnScoreChanged(Changed<Player> playerData) {
        OnScoreUpdated?.Invoke(playerData.Behaviour, playerData.Behaviour.Score);
    }
    
    private static void OnNameChanged(Changed<Player> playerData) {
        OnNameUpdated?.Invoke(playerData.Behaviour, playerData.Behaviour.NickName.ToString());
    }

    private static void OnPlayerAliveChanged(Changed<Player> playerData) {
        if (playerData.Behaviour.IsAlive) {
            playerData.Behaviour._playerVisuals.ShowPlayer();
            OnPlayerRespawned?.Invoke(playerData.Behaviour);
            return;
        }
        playerData.Behaviour._playerVisuals.DestroyedEffect();
    }

    private static void OnPlayerReadyChanged(Changed<Player> playerData) {
        Player playerWhoToggledReady = playerData.Behaviour;
        print($"Player ID: {playerWhoToggledReady} is READY SET TO: {playerWhoToggledReady.IsReady}");

        OnPlayerToggledReady?.Invoke(playerWhoToggledReady, playerWhoToggledReady.IsReady);
    }
    
    public void OnDamage(Bullet damager) {
        if (damager == null) {
            return;
        }

        if (_playerInvincibility == null || _playerInvincibility.IsInvincible) {
            print("Got hit while invincible!");
            return;
        }

        if (IsGameStateWhereScoreCanBeUpdated) {
            if (damager.Owner == this) {
                DecrementScoreBy(1);
            }
            else {
                damager.Owner.IncrementScoreBy(1);
            }
        }

        // TODO: Refactor
        IsAlive = false;
        _collider.enabled = false;
        _playerVisuals.DestroyedEffect();
        OnPlayerDestroyed?.Invoke(this);
        _respawnTimer = TickTimer.CreateFromSeconds(Runner, RESPAWN_DELAY_SECONDS);
    }

    private bool IsGameStateWhereScoreCanBeUpdated => GameStateManager.CurrentState == GameState.Game;
}
