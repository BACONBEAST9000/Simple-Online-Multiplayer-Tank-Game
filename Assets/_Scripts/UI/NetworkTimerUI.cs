using Fusion;
using TMPro;
using UnityEngine;

public class NetworkTimerUI : NetworkBehaviour {
    
    [SerializeField] private NetworkTimer _gameTimer;
    [SerializeField] private TMP_Text _timeText;

    private int _previousTimeLeft;
    private int _currentTimeLeft;

    public override void FixedUpdateNetwork() {
        _currentTimeLeft = (int)_gameTimer.GetTimeLeft();

        if (_currentTimeLeft != _previousTimeLeft) {
            _previousTimeLeft = _currentTimeLeft;
            UpdateTimeText();
        }
    }

    private void UpdateTimeText() {
        int minutes = Mathf.FloorToInt((float)(_currentTimeLeft / 60F));
        int seconds = Mathf.FloorToInt((float)(_currentTimeLeft - minutes * 60));

        string timeLeftString = string.Format("{00:00}:{1:00}", minutes, seconds);

        _timeText.text = timeLeftString;
    }
}
