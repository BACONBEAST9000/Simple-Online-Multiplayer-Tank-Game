using Fusion;
using System;
using UnityEngine;

// TODO: Refactor! Consider separating Score functionality from Player.
[RequireComponent(typeof(NetworkObject))]
public class Player : NetworkBehaviour, IDamageable {
    public const float RESPAWN_DELAY_SECONDS = 2;

    public static event Action<Player> OnPlayerDestroyed;
    public static event Action<Player> OnPlayerRespawned;

    public static event Action<Player, string> OnNameUpdated;
    public static event Action<Player> OnSpawned;
    public static event Action<Player> OnDespawned;


    [field: SerializeField] public PlayerVisuals Visuals { get; private set; }
    [field: SerializeField] public PlayerScore Scoring { get; private set; }
    [field: SerializeField] public PlayerReadyUp ReadyUp { get; private set; }
    

    [SerializeField] private Collider _collider;
    [SerializeField] private Hitbox _hitbox;
    [SerializeField] private PlayerInvincibility _playerInvincibility;
    

    [Networked(OnChanged = nameof(OnNameChanged))]
    [HideInInspector]
    public NetworkString<_16> NickName { get; private set; }

    [Networked(OnChanged = nameof(OnPlayerAliveChanged))]
    [HideInInspector]
    public NetworkBool IsAlive { get; private set; } = true;

    [Networked] private TickTimer _respawnTimer { get; set; }

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

    // TODO: Refactor
    private void CheckRespawnTimer() {
        if (!_respawnTimer.Expired(Runner)) {
            return;
        }

        SetAliveAndCollisionEnabled(true);

        _respawnTimer = default;
        RespawnManager.Instance.Respawn(this);
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void RpcSetNickName(string nickName) {
        if (string.IsNullOrEmpty(nickName)) return;
        NickName = nickName;
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
 
    public void OnDamage(Bullet bullet) {
        if (bullet == null || PlayerIsInvincible()) {
            return;
        }

        UpdateScores(bullet);

        OnDefeated();
    }

    private void UpdateScores(Bullet bullet) {
        if (!IsGameStateWhereScoreCanBeUpdated || bullet.Owner == null) {
            return;
        }

        if (bullet.Owner == this) {
            Scoring.DecrementScoreBy(1);
        }
        else {
            bullet.Owner.Scoring.IncrementScoreBy(1);
        }
    }

    // TODO: Refactor
    private void OnDefeated() {
        SetAliveAndCollisionEnabled(false);
        Visuals.DestroyedEffect();
        _respawnTimer = TickTimer.CreateFromSeconds(Runner, RESPAWN_DELAY_SECONDS);
        RPC_PlayerDefeated();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayerDefeated() {
        OnPlayerDestroyed?.Invoke(this);
    }
    
    private void SetAliveAndCollisionEnabled(bool value) {
        IsAlive = value;
        _collider.enabled = value;
        _hitbox.HitboxActive = value;
    }

    public bool IsHost => PlayerID == Runner.SessionInfo.MaxPlayers - 1;

    private bool PlayerIsInvincible() {
        return _playerInvincibility != null && _playerInvincibility.IsInvincible;
    }

    private bool IsGameStateWhereScoreCanBeUpdated => GameStateManager.CurrentState == GameState.Game;
}