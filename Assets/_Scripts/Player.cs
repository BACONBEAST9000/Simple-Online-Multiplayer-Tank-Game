using Fusion;
using System;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Player : NetworkBehaviour, IDamageable {
    public const float RESPAWN_DELAY_SECONDS = 2;

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

    public override void Spawned() {
        if (Object.HasInputAuthority) {
            var name = FindObjectOfType<PlayerData>().NickName;
            RpcSetNickName(name);
        }

        OnSpawned?.Invoke(Object.InputAuthority, this);
    }

    public override void FixedUpdateNetwork() {
        if (_respawnTimer.Expired(Runner)) {
            print("Respawn timer end");
            RespawnManager.Instance.Respawn(this);
            _respawnTimer = default;
        }
    }

    // RPC used to send player information to the Host
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    private void RpcSetNickName(string nickName) {
        if (string.IsNullOrEmpty(nickName)) return;
        NickName = nickName;

        print("Name changed to " + nickName);
    }

    public void IncrementScoreBy(int value) => Score += value;
    public void DecrementScoreBy(int value) => Score = Mathf.Max(0, Score - value);

    public static void OnScoreChanged(Changed<Player> playerData) {
        OnScoreUpdated?.Invoke(playerData.Behaviour.Object.InputAuthority, playerData.Behaviour.Score);
        //print("Player score changed! " + playerData.Behaviour.Score);
    }
    
    private static void OnNameChanged(Changed<Player> playerData) {
        OnNameUpdated?.Invoke(playerData.Behaviour.Object.InputAuthority, playerData.Behaviour.NickName.ToString());
        //print("Player name changed! " + playerData.Behaviour.NickName);
    }

    public void OnDamage(Bullet damager) {
        if (damager == null) {
            return;
        }

        if (damager.Owner == this) {
            DecrementScoreBy(1);
        }
        else {
            damager.Owner.IncrementScoreBy(1);
        }
        
        _respawnTimer = TickTimer.CreateFromSeconds(Runner, RESPAWN_DELAY_SECONDS);
    }
}