using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    [Header("References")]
    [SerializeField] private HandlePlayerInput _playerInput;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnTransform;

    [Header("Properties")]
    [SerializeField] private float _shootDelay = 0.5f;
    [SerializeField] private float _shootForce = 5f;

    [Header("Wall Detection")]
    [SerializeField] private float _checkIfWallRaycastDistance = 0.5f;
    [SerializeField] private LayerMask _wallLayer;

    private float _lastShotTime;

    private void OnEnable() {
        _playerInput.OnPlayerInput -= WhenPlayerPressesShootButton;
        _playerInput.OnPlayerInput += WhenPlayerPressesShootButton;
    }

    private void WhenPlayerPressesShootButton(PlayerInput input) {
        if ((input.Buttons & PlayerInput.SHOOT_INPUT) != 0 && NoCurrentShootDelay() && !WallAhead()) {
            Shoot();
            _lastShotTime = Time.time;
        }
    }

    private void OnDisable() {
        _playerInput.OnPlayerInput -= WhenPlayerPressesShootButton;
    }

    private bool NoCurrentShootDelay() {
        return Time.time - _lastShotTime > _shootDelay;
    }

    private bool WallAhead() {
        return Physics.Raycast(transform.position, transform.forward, out _, _checkIfWallRaycastDistance, _wallLayer);
    }

    private void Shoot() {
        Bullet bullet = Instantiate(_bulletPrefab, _bulletSpawnTransform.position, Quaternion.identity);
        bullet.Initalize(_bulletSpawnTransform.forward, _shootForce);
    }
}
