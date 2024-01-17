using Fusion;
using System;
using UnityEngine;

public class PlayerReadyUp : NetworkBehaviour {

    public static event Action<Player, bool> OnPlayerToggledReady;

    [SerializeField] private Player _player;

    [Networked(OnChanged = nameof(OnPlayerReadyChanged))]
    [HideInInspector]
    public NetworkBool IsReady { get; private set; }

    [Networked] private NetworkButtons PreviousButtons { get; set; }

    public override void Spawned() {
        IsReady = false;
    }

    public override void FixedUpdateNetwork() {
        if (GetInput(out PlayerInput input)) {
            if (IsReadyButtonPressed(input)) {
                IsReady = !IsReady;
            }
        }
    }

    private bool IsReadyButtonPressed(PlayerInput input) {
        return input.Buttons.WasPressed(PreviousButtons, ActionButtons.Ready);
    }

    private static void OnPlayerReadyChanged(Changed<PlayerReadyUp> changed) {
        PlayerReadyUp playerWhoToggledReady = changed.Behaviour;
        print($"Player ID: {playerWhoToggledReady} is READY SET TO: {playerWhoToggledReady.IsReady}");

        OnPlayerToggledReady?.Invoke(changed.Behaviour._player, playerWhoToggledReady.IsReady);
    }
}
