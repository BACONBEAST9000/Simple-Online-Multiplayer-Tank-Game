using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private RectTransform _gameEndScreen;
    [SerializeField] private NetworkTimer _gameTimer;
    [SerializeField] private bool _testingStopGameFromEndng = false;

    private void OnEnable() {
        _gameTimer.OnTimerEnd -= WhenGameTimerEnds;
        _gameTimer.OnTimerEnd += WhenGameTimerEnds;
    }

    private void OnDisable() {
        _gameTimer.OnTimerEnd -= WhenGameTimerEnds;
    }

    private void WhenGameTimerEnds() {
        if (_testingStopGameFromEndng)
            return;
        
        _gameEndScreen.gameObject.SetActive(true);
        GameStateManager.ChangeState(GameState.GameEnd);
    }
}
