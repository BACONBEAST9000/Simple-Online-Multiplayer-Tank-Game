using Fusion;
using UnityEngine;

public struct PlayerInput : INetworkInput{
    public Vector2 MoveInput;
    public NetworkButtons Buttons;
}

enum TankButtons {
    Ready = 0,
    Shoot = 1,
}