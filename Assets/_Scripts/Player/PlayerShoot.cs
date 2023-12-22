using Fusion;
using System;
using UnityEngine;

public class PlayerShoot : NetworkBehaviour {

    public static event Action<Bullet, Player> OnPlayerShotBullet;

    [Header("References")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnTransform;
    [SerializeField] private Player _player;

    [Header("Properties")]
    [SerializeField] private float _shootDelaySeconds = 0.5f;
    [SerializeField] private float _shootForce = 5f;

    [Header("Wall Detection")]
    [SerializeField] private float _checkIfWallRaycastDistance = 0.5f;
    [SerializeField] private LayerMask _wallLayer;

    [Networked] private TickTimer ShootDelay { get; set; }
    [Networked] private NetworkButtons _previousButtons { get; set; }

    public override void FixedUpdateNetwork() {
        if (!GetInput(out PlayerInput input)) {
            return;
        }

        if (ShouldShoot(input)) {
            Shoot();
            ShootDelay = TickTimer.CreateFromSeconds(Runner, _shootDelaySeconds);
        }
    }

    private bool ShouldShoot(PlayerInput input) => IsGameStateWhereCanShoot() && NoShootDelay() && ShootButtonPressed(input) && NoWallAhead();

    private bool IsGameStateWhereCanShoot() => GameStateManager.CurrentState != GameState.GameEnd;

    private bool ShootButtonPressed(PlayerInput input) => input.Buttons.WasPressed(_previousButtons, ActionButtons.Shoot);

    private bool NoShootDelay() => ShootDelay.ExpiredOrNotRunning(Runner);

    private bool NoWallAhead() => !Physics.Raycast(transform.position, transform.forward, out _, _checkIfWallRaycastDistance, _wallLayer);

    private void Shoot() {
        Runner.Spawn(
                    _bulletPrefab,
                    _bulletSpawnTransform.position,
                    Quaternion.identity,
                    Object.InputAuthority,
                    (runner, networkObject) => {
                        Bullet bulletToShoot = networkObject.GetComponent<Bullet>();
                        bulletToShoot.Initialize(_bulletSpawnTransform.forward, _shootForce, _player);
                        OnPlayerShotBullet?.Invoke(bulletToShoot, _player);
                    }
                );
    }
}
