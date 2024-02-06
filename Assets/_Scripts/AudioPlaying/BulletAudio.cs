using UnityEngine;

public class BulletAudio : AudioPlayingInstance {

    [Header("References")]
    [SerializeField] private Bullet _bullet;

    [Header("Sounds")]
    [SerializeField] private AudioClip _hitWallSound;

    private void OnEnable() {
        Bullet.OnLocalCollisionHitWall -= WhenABulletHitsWall;
        Bullet.OnLocalCollisionHitWall += WhenABulletHitsWall;
    }

    private void OnDisable() {
        Bullet.OnLocalCollisionHitWall -= WhenABulletHitsWall;
    }

    private void WhenABulletHitsWall(Bullet bulletThatHitWall) {
        if (BulletIsNotThisBullet(bulletThatHitWall)) return;

        soundEmitter.Play(_hitWallSound);
    }

    private bool BulletIsNotThisBullet(Bullet bullet) => bullet != _bullet;
}
