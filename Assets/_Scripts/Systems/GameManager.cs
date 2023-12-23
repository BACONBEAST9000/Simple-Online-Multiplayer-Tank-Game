using Fusion;
using UnityEngine;

public class GameManager : NetworkBehaviour {

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
        
        RPC_EndGame();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_EndGame() {
        GameStateManager.ChangeState(GameState.GameEnd);
    }
}
