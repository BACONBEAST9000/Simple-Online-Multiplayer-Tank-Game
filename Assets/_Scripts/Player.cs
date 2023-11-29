using Fusion;
using System;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class Player : NetworkBehaviour, IDamageable {

    [Networked(OnChanged = nameof(ScoreChanged))]
    public int Score { get; private set; }
    
    public PlayerData Data { get; private set; }

    public override void Spawned() {
        Data = new PlayerData {
            PlayerName = "Player" + UnityEngine.Random.Range(0, 10000),
            Lives = 3
        };
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Score++;
        }
    }

    public static void ScoreChanged(Changed<Player> playerData) {
        print("Player data changed!");
    }

    public void OnDamage() {
        Data.DecrementLivesBy(1);
    }
}

public struct PlayerData {
    public string PlayerName;
    public int Lives;

    public void IncrementLivesBy(int amount) {
        if (amount < 0) {
            throw new ArgumentOutOfRangeException("amount");
        }
        
        UpdateLivesBy(amount);
    }
    
    public void DecrementLivesBy(int amount) {
        if (amount < 0) {
            throw new ArgumentOutOfRangeException("amount");
        }

        UpdateLivesBy(-amount);
    }

    public void UpdateLivesBy(int amount) {
        Lives = Mathf.Max(0, Lives + amount);
    }
}
