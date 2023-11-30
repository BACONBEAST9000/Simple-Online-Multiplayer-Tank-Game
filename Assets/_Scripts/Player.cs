using Fusion;
using System;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Player : NetworkBehaviour, IDamageable {

    public static event Action<PlayerRef, int> OnScoreUpdated;
    public static event Action<PlayerRef, Player> OnSpawned;

    [Networked(OnChanged = nameof(ScoreChanged))]
    [HideInInspector]
    public int Score { get; private set; }
    
    public PlayerData Data { get; private set; }

    public override void Spawned() {
        Data = new PlayerData {
            PlayerName = "Player" + UnityEngine.Random.Range(0, 10000),
            Points = 0
        };

        OnSpawned?.Invoke(Object.InputAuthority, this);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Score++;
        }
    }

    public static void ScoreChanged(Changed<Player> playerData) {
        OnScoreUpdated?.Invoke(playerData.Behaviour.Object.InputAuthority, playerData.Behaviour.Score);
        print("Player score changed! " + playerData.Behaviour.Score);
    }

    public void OnDamage(Bullet damager) {
        if (damager == null) {
            return;
        }

        if (damager.Owner == this) {
            Score--;
            print("Self damage. Score now " + Score);
        }
        
        
        print($"Damaged by {damager.Owner} for {damager.Damage} damage!");
    }
}

public struct PlayerData {
    public string PlayerName;
    public int Points;

    public void IncrementPointsBy(int amount) {
        if (amount < 0) {
            throw new ArgumentOutOfRangeException("amount");
        }
        
        UpdatePointsBy(amount);
    }
    
    public void DecrementPointsBy(int amount) {
        if (amount < 0) {
            throw new ArgumentOutOfRangeException("amount");
        }

        UpdatePointsBy(-amount);
    }

    public void UpdatePointsBy(int amount) {
        Points = Mathf.Max(0, Points + amount);
    }
}
