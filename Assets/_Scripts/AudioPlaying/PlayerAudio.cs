using UnityEngine;

public class PlayerAudio : AudioPlayingInstance {

    [Header("Requirements")]
    [SerializeField] private Player _player;

    [Header("Sounds")]
    [SerializeField] private AudioClip _shootSound;

    private void OnEnable() {
        PlayerShoot.OnPlayerShotBullet -= WhenAPlayerShootsBullet;
        PlayerShoot.OnPlayerShotBullet += WhenAPlayerShootsBullet;
    }

    private void OnDisable() {
        PlayerShoot.OnPlayerShotBullet -= WhenAPlayerShootsBullet;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            soundEmitter.Play(_shootSound);
        }
    }

    private void WhenAPlayerShootsBullet(Bullet bullet, Player playerWhoShot) {
        if (PlayerIsNotThisPlayer(playerWhoShot)) return;

        soundEmitter.Play(_shootSound);
    }

    private bool PlayerIsNotThisPlayer(Player player) => _player != player;
}
