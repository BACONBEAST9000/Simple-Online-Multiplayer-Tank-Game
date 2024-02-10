using UnityEngine;

public class PlayerAudio : AudioPlayingInstance {

    [Header("Requirements")]
    [SerializeField] private Player _player;

    [Header("Sounds")]
    [SerializeField] private AudioClip _destroyedSound;

    private void OnEnable() {
        Player.OnPlayerDestroyed -= WhenAPlayerDestroyed;
        Player.OnPlayerDestroyed += WhenAPlayerDestroyed;
    }

    private void OnDisable() {
        Player.OnPlayerDestroyed -= WhenAPlayerDestroyed;
    }
    
    private void WhenAPlayerDestroyed(Player destroyedPlayer) {
        if (PlayerIsNotThisPlayer(destroyedPlayer)) return;

        soundEmitter.Play(_destroyedSound);
    }

    private bool PlayerIsNotThisPlayer(Player player) => _player != player;
}
