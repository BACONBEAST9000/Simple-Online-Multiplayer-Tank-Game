using Fusion;
using UnityEngine;

public struct PlayerInput : INetworkInput{
    public const byte SHOOT_INPUT = 0x01;

    public byte Buttons;

    public Vector2 MoveInput;
}
