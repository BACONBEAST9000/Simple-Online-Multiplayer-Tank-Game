using Fusion;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour {

    [Header("References")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnTransform;

    [Header("Properties")]
    [SerializeField] private float _shootDelaySeconds = 0.5f;
    [SerializeField] private float _shootForce = 5f;

    [Header("Wall Detection")]
    [SerializeField] private float _checkIfWallRaycastDistance = 0.5f;
    [SerializeField] private LayerMask _wallLayer;

    [Networked] private TickTimer ShootDelay { get; set; }

    public override void FixedUpdateNetwork() {
        if (!GetInput(out PlayerInput input)) {
            return;
        }

        if (ShouldShoot(input)) {
            Shoot();
            ShootDelay = TickTimer.CreateFromSeconds(Runner, _shootDelaySeconds);
        }
    }

    private bool ShouldShoot(PlayerInput input) => NoShootDelay() && ShootButtonPressed(input) && NoWallAhead();

    private static bool ShootButtonPressed(PlayerInput input) => (input.Buttons & PlayerInput.SHOOT_INPUT) != 0;

    private bool NoShootDelay() => ShootDelay.ExpiredOrNotRunning(Runner);

    private bool NoWallAhead() => !Physics.Raycast(transform.position, transform.forward, out _, _checkIfWallRaycastDistance, _wallLayer);

    private void Shoot() {
        Runner.Spawn(
                    _bulletPrefab,
                    _bulletSpawnTransform.position,
                    Quaternion.identity,
                    Object.InputAuthority,
                    (runner, o) => {
                        o.GetComponent<Bullet>().Initialize(_bulletSpawnTransform.forward, _shootForce);
                    }
                );
    }
}
