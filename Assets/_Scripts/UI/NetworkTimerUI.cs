using Fusion;
using TMPro;
using UnityEngine;

public abstract class NetworkTimerUI : NetworkBehaviour {
    
    [SerializeField] protected NetworkTimer _gameTimer;
    [SerializeField] protected TMP_Text _timeText;

    protected int _previousTimeLeft;
    protected int _currentTimeLeft;

    public override void FixedUpdateNetwork() {
        if (_gameTimer == null) return;

        UpdateTime();
    }

    private void UpdateTime() {
        _currentTimeLeft = (int)_gameTimer?.GetTimeLeft();

        if (_currentTimeLeft != _previousTimeLeft) {
            _previousTimeLeft = _currentTimeLeft;
            UpdateTimeText();
        }
    }

    protected abstract void UpdateTimeText();
}
