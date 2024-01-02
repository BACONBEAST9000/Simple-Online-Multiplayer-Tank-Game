using UnityEngine;

public class GameTimerUI : NetworkTimerUI {

    protected override void UpdateTimeText() {
        int minutes = Mathf.FloorToInt((float)(_currentTimeLeft / 60F));
        int seconds = Mathf.FloorToInt((float)(_currentTimeLeft - minutes * 60));

        string timeLeftString = string.Format("{00:00}:{1:00}", minutes, seconds);

        _timeText.text = timeLeftString;
    }

}
