using Fusion;
using System;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Player : NetworkBehaviour, IDamageable {

    public static event Action<PlayerRef, int> OnScoreUpdated;
    public static event Action<PlayerRef, string> OnNameUpdated;
    public static event Action<PlayerRef, Player> OnSpawned;

    [Networked(OnChanged = nameof(OnScoreChanged))]
    [HideInInspector]
    public int Score { get; private set; }

    [HideInInspector]
    [Networked(OnChanged = nameof(OnNameChanged))]
    public NetworkString<_16> NickName { get; private set; }

    public override void Spawned() {
        if (Object.HasInputAuthority) {
            var name = FindObjectOfType<PlayerData>().NickName;
            RpcSetNickName(name);
        }

        OnSpawned?.Invoke(Object.InputAuthority, this);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Score++;
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

    public static void OnScoreChanged(Changed<Player> playerData) {
        OnScoreUpdated?.Invoke(playerData.Behaviour.Object.InputAuthority, playerData.Behaviour.Score);
        print("Player score changed! " + playerData.Behaviour.Score);
    }
    
    private static void OnNameChanged(Changed<Player> playerData) {
        OnNameUpdated?.Invoke(playerData.Behaviour.Object.InputAuthority, playerData.Behaviour.NickName.ToString());
        print("Player name changed! " + playerData.Behaviour.NickName);
    }

    public void OnDamage(Bullet damager) {
        if (damager == null) {
            return;
        }

        if (damager.Owner == this) {
            Score--;
            print("Self damage. Score now " + Score);
            return;
        }
        
        damager.Owner.IncrementScoreBy(1);
        print($"Damaged by {damager.Owner} for {damager.Damage} damage!");
    }
}