using UnityEngine;

public class PlayerAudio : AudioPlayingInstance {

    [Header("Requirements")]
    [SerializeField] private Player _player;

    [Header("Sounds")]
    [SerializeField] private AudioClip _shootSound;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            soundEmitter.Play(_shootSound);
        }
    }

    private bool PlayerIsNotThisPlayer(Player player) => _player != player;
}
