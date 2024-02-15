using Fusion;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private const string SHOOT_TRIGGER_NAME = "Shoot";

    [SerializeField] private Animator _animator;
    [SerializeField] private Player _player;

    private void OnEnable() {
        PlayerShoot.OnPlayerShotBullet -= WhenAPlayerShotBullet;
        PlayerShoot.OnPlayerShotBullet += WhenAPlayerShotBullet;
    }

    private void OnDisable() {
        PlayerShoot.OnPlayerShotBullet -= WhenAPlayerShotBullet;
    }

    private void WhenAPlayerShotBullet(Bullet bullet, Player playerWhoShot) {
        if (playerWhoShot != _player) return;

        _animator.SetTrigger(SHOOT_TRIGGER_NAME);
    }
}
