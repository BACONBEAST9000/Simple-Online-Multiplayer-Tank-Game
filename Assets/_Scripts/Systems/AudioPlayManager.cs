using UnityEngine;

public class AudioPlayManager : MonoBehaviour {
    
    public static AudioPlayManager Instance;

    [SerializeField] private AudioSource _audioSource;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Play(AudioClip clip) {
        _audioSource.PlayOneShot(clip);
    }
}