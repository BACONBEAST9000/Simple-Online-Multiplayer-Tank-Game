using UnityEngine;

public class StartGameTimerUI : NetworkTimerUI {

    [SerializeField] private AudioClip _countdownSound;

    private void OnEnable() {
        _gameTimer.OnTimerEnd -= WhenCountdownEnds;
        _gameTimer.OnTimerEnd += WhenCountdownEnds;
    }

    private void OnDisable() {
        _gameTimer.OnTimerEnd -= WhenCountdownEnds;
    }

    private void WhenCountdownEnds() {
        HideCountdownTimer();
    }

    private void HideCountdownTimer() => _timeText.gameObject.SetActive(false);

    protected override void UpdateTimeText() {
        _timeText.text = Mathf.CeilToInt(_gameTimer.GetTimeLeft()).ToString();
        AudioPlayManager.Instance.Play(_countdownSound);
    }
}