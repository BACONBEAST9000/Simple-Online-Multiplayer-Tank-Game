using Fusion;
using System;
using UnityEngine;

public class Player : NetworkBehaviour, IDamageable {

    private PlayerData _playerData;
    public PlayerData Data {
        get => _playerData;
        private set => _playerData = value;
    }

    public override void Spawned() {
        Data = new PlayerData {
            PlayerName = "Player",
            Lives = 3
        };
    }

    public void OnDamage() {
        _playerData.DecrementLivesBy(1);
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
