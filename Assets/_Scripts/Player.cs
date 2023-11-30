using Fusion;
using System;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Player : NetworkBehaviour, IDamageable {

    [Networked(OnChanged = nameof(ScoreChanged))]
    [HideInInspector]
    public int Score { get; private set; }
    
    public PlayerData Data { get; private set; }

    public override void Spawned() {
        Data = new PlayerData {
            PlayerName = "Player" + UnityEngine.Random.Range(0, 10000),
            Points = 3
        };
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Score++;
        }
    }

    public static void ScoreChanged(Changed<Player> playerData) {
        print("Player score changed! " + playerData.Behaviour.Score);
    }

    public void OnDamage(IDamage damager) {
        print("Damage: " + damager.Damage);
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
