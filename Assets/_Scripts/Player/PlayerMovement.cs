using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour {

    [Header("References")]
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Properties")]
    [SerializeField] private float _maxMoveSpeed = 5f;
    [SerializeField] private float _minMoveSpeed = 5f;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _deceleration = 3f;
    [SerializeField] private float _rotateSpeed = 180f;

    private float _currentMoveSpeed = 0f;
    private Vector2 _moveInput;

    private bool _canMove = true;

    private void OnEnable() {
        Player.OnPlayerDestroyed -= WhenPlayerDestroyed;
        Player.OnPlayerDestroyed += WhenPlayerDestroyed;

        Player.OnPlayerRespawned -= WhenPlayerRespawned;
        Player.OnPlayerRespawned += WhenPlayerRespawned;
    }

    private void OnDisable() {
        Player.OnPlayerDestroyed -= WhenPlayerDestroyed;
        Player.OnPlayerRespawned -= WhenPlayerRespawned;
    }

    private void WhenPlayerDestroyed(PlayerRef playerRef) {
        if (playerRef != Object.InputAuthority) return;

        _canMove = false;
    }
    
    private void WhenPlayerRespawned(PlayerRef playerRef) {
        if (playerRef != Object.InputAuthority) return;

        _canMove = true;
    }

    public override void Spawned() {
        _canMove = true;
    }

    public override void FixedUpdateNetwork() {
        if (!_canMove) return;
        
        if (!GetInput(out PlayerInput input)) {
            _moveInput = Vector2.zero;
            return;
        }

        _moveInput = input.MoveInput;

        HandleTurning();
        HandleAcceleration();
    }

    private void HandleTurning() {
        float rotation = _moveInput.x * _rotateSpeed * Runner.DeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, rotation, 0f);
        _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
    }

    private void HandleAcceleration() {
        float targetMoveSpeed = _moveInput.y * ((_moveInput.y > 0) ? _maxMoveSpeed : _minMoveSpeed);

        float moveAmount = (targetMoveSpeed > _currentMoveSpeed ? _acceleration : _deceleration) * Runner.DeltaTime;

        _currentMoveSpeed = Mathf.MoveTowards(_currentMoveSpeed, targetMoveSpeed, moveAmount);

        _rigidbody.velocity = transform.forward * _currentMoveSpeed;
    }

    private void OnCollisionEnter(Collision collision) {
        _currentMoveSpeed *= 0.5f;
    }
}
