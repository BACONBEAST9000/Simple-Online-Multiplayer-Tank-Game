using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private RectTransform _gameEndScreen;
    [SerializeField] private NetworkTimer _gameTimer;

    private void OnEnable() {
        _gameTimer.OnTimerEnd -= WhenGameTimerEnds;
        _gameTimer.OnTimerEnd += WhenGameTimerEnds;
    }

    private void OnDisable() {
        _gameTimer.OnTimerEnd -= WhenGameTimerEnds;
    }

    private void WhenGameTimerEnds() {
        _gameEndScreen.gameObject.SetActive(true);
    }
}
