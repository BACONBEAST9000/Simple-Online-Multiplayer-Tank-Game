using UnityEngine;

public class StartCountdown : MonoBehaviour {

    [SerializeField] private NetworkTimer _networkTimerForGameStart;
    [SerializeField] private NetworkTimer _gameTimerObject;

    private void OnEnable() {
        _networkTimerForGameStart.OnTimerEnd -= WhenTimerEnds;
        _networkTimerForGameStart.OnTimerEnd += WhenTimerEnds;
    }

    private void OnDisable() {
        _networkTimerForGameStart.OnTimerEnd -= WhenTimerEnds;
    }

    private void WhenTimerEnds() {
        GameStateManager.ChangeState(GameState.Game);
        _gameTimerObject.gameObject.SetActive(true);
    }
}
