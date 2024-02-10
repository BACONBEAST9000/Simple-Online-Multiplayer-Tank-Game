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
        Player.OnPlayerDestroyed += WhenPlayerDestroyed;
        Player.OnPlayerRespawned += WhenPlayerRespawned;
        GameStateManager.OnStateChanged += WhenGameStateChanges;
    }


    private void OnDisable() {
        Player.OnPlayerDestroyed -= WhenPlayerDestroyed;
        Player.OnPlayerRespawned -= WhenPlayerRespawned;
        GameStateManager.OnStateChanged -= WhenGameStateChanges;
    }
    
    private void WhenGameStateChanges(GameState newState) {
        UpdateCanMove();
    }

    private void WhenPlayerDestroyed(Player destroyedPlayer) {
        if (PlayerIsNotThisPlayer(destroyedPlayer)) return;

        _canMove = false;
    }


    private void WhenPlayerRespawned(Player respawnedPlayer) {
        if (PlayerIsNotThisPlayer(respawnedPlayer)) return;

        UpdateCanMove();
    }

    private bool PlayerIsNotThisPlayer(Player player) => player.PlayerID != Object.InputAuthority;

    private static bool ShouldBeAbleToMove() {
        return (GameStateManager.CurrentState != GameState.PreGameStart);
    }

    private void UpdateCanMove() => _canMove = ShouldBeAbleToMove();

    public override void FixedUpdateNetwork() {
        HandleMovement();
    }

    private void HandleMovement() {
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
        float speedMoveTowards = (_moveInput.y > 0) ? _maxMoveSpeed : _minMoveSpeed;
        float targetMoveSpeed = _moveInput.y * speedMoveTowards;

        float moveAmount = (targetMoveSpeed > _currentMoveSpeed ? _acceleration : _deceleration) * Runner.DeltaTime;

        _currentMoveSpeed = Mathf.MoveTowards(_currentMoveSpeed, targetMoveSpeed, moveAmount);

        _rigidbody.velocity = transform.forward * _currentMoveSpeed;
    }

    private void OnCollisionEnter(Collision collision) {
        _currentMoveSpeed *= 0.5f;
    }
}
