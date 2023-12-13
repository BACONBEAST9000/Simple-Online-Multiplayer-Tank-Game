using Fusion;
using System;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Player : NetworkBehaviour, IDamageable {
    public const float RESPAWN_DELAY_SECONDS = 2;

    public static event Action<PlayerRef> OnPlayerDestroyed;
    public static event Action<PlayerRef> OnPlayerRespawned;
    public static event Action<Player, bool> OnPlayerToggledReady;

    public static event Action<PlayerRef, int> OnScoreUpdated;
    public static event Action<PlayerRef, string> OnNameUpdated;
    public static event Action<PlayerRef, Player> OnSpawned;

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
    [SerializeField] public PlayerVisuals TestVisuals;

    [Networked]
    public bool IsReady { get; private set; }

    [Networked] private NetworkButtons _previousButtons { get; set; }

    public int PlayerID { get; private set; }

    public override void Spawned() {
        IsAlive = true;
        PlayerID = Object.InputAuthority;

        if (Object.HasInputAuthority) {
            var name = FindObjectOfType<PlayerData>().NickName;
            RpcSetNickName(name);
        }

        print($"I, {NickName} have just spawned. Time to invoke OnSpawned event!");
        OnSpawned?.Invoke(Object.InputAuthority, this);

        PlayerManager.AddPlayer(this);
    }

    public override void Despawned(NetworkRunner runner, bool hasState) {
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
                OnPlayerToggledReady?.Invoke(this, IsReady);
                print($"Player ID: {Object.InputAuthority} is READY SET TO: {IsReady}");
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
        OnScoreUpdated?.Invoke(playerData.Behaviour.Object.InputAuthority, playerData.Behaviour.Score);
    }
    
    private static void OnNameChanged(Changed<Player> playerData) {
        OnNameUpdated?.Invoke(playerData.Behaviour.Object.InputAuthority, playerData.Behaviour.NickName.ToString());
    }

    private static void OnPlayerAliveChanged(Changed<Player> playerData) {
        if (playerData.Behaviour.IsAlive) {
            playerData.Behaviour.TestVisuals.ShowPlayer();
            OnPlayerRespawned?.Invoke(playerData.Behaviour.Object.InputAuthority);
            return;
        }
        playerData.Behaviour.TestVisuals.DestroyedEffect();
    }

    
    public void OnDamage(Bullet damager) {
        if (damager == null) {
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
        TestVisuals.DestroyedEffect();
        OnPlayerDestroyed?.Invoke(Object.InputAuthority);
        _respawnTimer = TickTimer.CreateFromSeconds(Runner, RESPAWN_DELAY_SECONDS);
    }

    private bool IsGameStateWhereScoreCanBeUpdated => GameStateManager.CurrentState == GameState.Game;
}
