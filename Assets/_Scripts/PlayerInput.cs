using Fusion;
using UnityEngine;

public struct PlayerInput : INetworkInput{
    public Vector2 MoveInput;
    public NetworkButtons Buttons;
}

enum TankButtons {
    Shoot = 0,
}