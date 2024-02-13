using UnityEngine;

public class ButtonSFX : MonoBehaviour {

    [SerializeField] private AudioClip _hoverSound;
    [SerializeField] private AudioClip _selectSound;

    public void PlayHoverSound() => AudioPlayManager.Instance.Play(_hoverSound);
    public void PlaySelectSound() => AudioPlayManager.Instance.Play(_selectSound);
}
