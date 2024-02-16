using UnityEngine;

public class SoundEmitter : MonoBehaviour {

    [SerializeField] private AudioSource _audioSource;

    public void Play(AudioClip audioClip) {
        _audioSource.PlayOneShot(audioClip);
    }
}
