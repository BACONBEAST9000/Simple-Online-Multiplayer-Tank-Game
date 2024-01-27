using Fusion;
using System;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Player : NetworkBehaviour, IDamageable {
    public const float RESPAWN_DELAY_SECONDS = 2;

    public static event Action<Player> OnPlayerDestroyed;
    public static event Action<Player> OnPlayerRespawned;

    public static event Action<Player, int> OnScoreUpdated;
    public static event Action<Player, string> OnNameUpdated;
    public static event Action<Player> OnSpawned;
    public static event Action<Player> OnDespawned;

    [field: SerializeField] public PlayerVisuals Visuals { get; private set; }
    
    [SerializeField] private Collider _collider;
    [SerializeField] private PlayerInvincibility _playerInvincibility;
    [field: SerializeField] public PlayerReadyUp ReadyUp { get; private set; }

    [Networked(OnChanged = nameof(OnScoreChanged))]
    [HideInInspector]
    public int Score { get; private set; }

    [Networked(OnChanged = nameof(OnNameChanged))]
    [HideInInspector]
    public NetworkString<_16> NickName { get; private set; }

    [Networked] private TickTimer _respawnTimer { get; set; }

    [Networked(OnChanged = nameof(OnPlayerAliveChanged))]
    [HideInInspector]
    public NetworkBool IsAlive { get; private set; } = true;

    public int PlayerID { get; private set; }


    public override void Spawned() {
        IsAlive = true;
        
        PlayerID = Object.InputAuthority;

        if (Object.HasInputAuthority) {
            var name = FindObjectOfType<LocalPlayerData>().NickName;
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
        CheckRespawnTimer();
    }

    private void CheckRespawnTimer() {
        if (!_respawnTimer.Expired(Runner)) {
            return;
        }

        IsAlive = true;
        _collider.enabled = true;

        _respawnTimer = default;
        RespawnManager.Instance.Respawn(this);
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
            playerData.Behaviour.Visuals.ShowPlayer();
            OnPlayerRespawned?.Invoke(playerData.Behaviour);
            return;
        }
        playerData.Behaviour.Visuals.DestroyedEffect();
    }

    
    
    public void OnDamage(Bullet damager) {
        if (damager == null || PlayerIsInvincible()) {
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

        OnDefeated();
    }

    // TODO: Refactor
    private void OnDefeated() {
        IsAlive = false;
        _collider.enabled = false;
        Visuals.DestroyedEffect();
        OnPlayerDestroyed?.Invoke(this);
        _respawnTimer = TickTimer.CreateFromSeconds(Runner, RESPAWN_DELAY_SECONDS);
    }
    
    public bool IsHost => PlayerID == Runner.SessionInfo.MaxPlayers - 1;

    private bool PlayerIsInvincible() {
        return _playerInvincibility != null && _playerInvincibility.IsInvincible;
    }

    private bool IsGameStateWhereScoreCanBeUpdated => GameStateManager.CurrentState == GameState.Game;
}
