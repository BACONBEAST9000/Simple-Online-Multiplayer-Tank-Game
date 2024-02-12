using UnityEngine;

public class StartCountdown : MonoBehaviour {

    [SerializeField] private NetworkTimer _networkTimerForGameStart;
    [SerializeField] private NetworkTimer _gameTimerObject;
    [SerializeField] private RectTransform _countdownUI;

    private void OnEnable() {
        _networkTimerForGameStart.OnTimerEnd -= WhenTimerEnds;
        _networkTimerForGameStart.OnTimerEnd += WhenTimerEnds;
    }

    private void OnDisable() {
        _networkTimerForGameStart.OnTimerEnd -= WhenTimerEnds;
    }

    private void WhenTimerEnds() {
        GameStateManager.ChangeState(GameState.Game);
        HideCountdownUI();
        SetGameTimerActive();
    }

    private void SetGameTimerActive() => _gameTimerObject.gameObject.SetActive(true);

    private void HideCountdownUI() => _countdownUI.gameObject.SetActive(false);
}
