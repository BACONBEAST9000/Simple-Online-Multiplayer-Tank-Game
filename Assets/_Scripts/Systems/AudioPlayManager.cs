using UnityEngine;

public class AudioPlayManager : SingletonPersistent<AudioPlayManager> {
    
    [SerializeField] private AudioSource _audioSource;

    public void Play(AudioClip clip) {
        _audioSource.PlayOneShot(clip);
    }
}