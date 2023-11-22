using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour {

    [Header("References")]
    [SerializeField] private HandlePlayerInput _playerInput;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Properties")]
    [SerializeField] private float _maxMoveSpeed = 5f;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _deceleration = 3f;
    [SerializeField] private float _rotateSpeed = 180f;

    private float _currentMoveSpeed = 0f;

    //private void OnEnable() {
    //    _playerInput.OnPlayerInput -= MoveAndRotate;
    //    _playerInput.OnPlayerInput += MoveAndRotate;
    //}

    //private void OnDisable() {
    //    _playerInput.OnPlayerInput -= MoveAndRotate;
    //}

    public override void FixedUpdateNetwork() {
        if (!GetInput(out PlayerInput input)) {
            return;
        }

        MoveAndRotate(input);
    }

    private void MoveAndRotate(PlayerInput playerInput) {
        Vector2 moveInput = playerInput.MoveInput;
        
        float rotation = moveInput.x * _rotateSpeed * Runner.DeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotation, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);


        float targetSpeed = moveInput.y * _maxMoveSpeed;
        
        _currentMoveSpeed = Mathf.MoveTowards(_currentMoveSpeed, targetSpeed,
            (targetSpeed > _currentMoveSpeed ? _acceleration : _deceleration) * Runner.DeltaTime);

        _rigidbody.velocity = transform.forward * _currentMoveSpeed;
    }
}
