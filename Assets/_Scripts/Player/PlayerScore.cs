using Fusion;
using System;
using UnityEngine;

public class PlayerScore : NetworkBehaviour {
    public static event Action<Player, int> OnScoreUpdated;

    [SerializeField] private Player _player;

    [Networked(OnChanged = nameof(OnScoreChanged))]
    [HideInInspector]
    public int Score { get; private set; }

    public void IncrementScoreBy(int value) => Score += value;
    public void DecrementScoreBy(int value) => Score = Mathf.Max(0, Score - value);

    public static void OnScoreChanged(Changed<PlayerScore> changed) {
        OnScoreUpdated?.Invoke(changed.Behaviour._player, changed.Behaviour.Score);
    }
}
