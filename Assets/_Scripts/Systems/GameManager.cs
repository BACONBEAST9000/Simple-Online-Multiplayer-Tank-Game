using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour {

    [SerializeField] private NetworkTimer _gameTimer;
    [SerializeField] private NetworkTimer _endScreenTimer;
    [SerializeField] private bool _testingStopGameFromEndng = false;

    private void OnEnable() {
        _gameTimer.OnTimerEnd -= WhenGameTimerEnds;
        _gameTimer.OnTimerEnd += WhenGameTimerEnds;

        _endScreenTimer.OnTimerEnd -= WhenEndScreenTimerEnds;
        _endScreenTimer.OnTimerEnd += WhenEndScreenTimerEnds;
    }

    private void OnDisable() {
        _gameTimer.OnTimerEnd -= WhenGameTimerEnds;
        _endScreenTimer.OnTimerEnd -= WhenEndScreenTimerEnds;
    }

    private void WhenGameTimerEnds() {
        if (_testingStopGameFromEndng)
            return;
        
        RPC_EndGame();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_EndGame() {
        _gameTimer.StopTimer();
        GameStateManager.ChangeState(GameState.GameEnd);
    }
    
    private void WhenEndScreenTimerEnds() {
        MultiplayerSessionManager.Instance.ShutdownSession();
    }
}
