using Fusion;
using UnityEngine;

public class GameHandler : NetworkBehaviour {

    [SerializeField] private NetworkTimer _gameTimer;
    [SerializeField] private NetworkTimer _endScreenTimer;

    private void OnEnable() {
        GameStateManager.OnStateChanged -= WhenGameStateChanges;
        GameStateManager.OnStateChanged += WhenGameStateChanges;

        _gameTimer.OnTimerEnd -= WhenGameTimerEnds;
        _gameTimer.OnTimerEnd += WhenGameTimerEnds;

        _endScreenTimer.OnTimerEnd -= WhenEndScreenTimerEnds;
        _endScreenTimer.OnTimerEnd += WhenEndScreenTimerEnds;

        PlayerManager.OnPlayerListUpdated -= WhenPlayerListUpdated;
        PlayerManager.OnPlayerListUpdated += WhenPlayerListUpdated;
    }


    private void OnDisable() {
        GameStateManager.OnStateChanged -= WhenGameStateChanges;
        _gameTimer.OnTimerEnd -= WhenGameTimerEnds;
        _endScreenTimer.OnTimerEnd -= WhenEndScreenTimerEnds;
        PlayerManager.OnPlayerListUpdated -= WhenPlayerListUpdated;
    }

    private void WhenGameStateChanges(GameState newState) {
        if (newState == GameState.Game) {
            EndGameIfNotEnoughPlayers();
        }
    }

    private void WhenGameTimerEnds() {
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
    
    private void WhenPlayerListUpdated() {
        EndGameIfNotEnoughPlayers();
    }

    private void EndGameIfNotEnoughPlayers() {
        if (NotEnoughPlayersInGame()) {
            RPC_EndGame();
        }
    }

    private static bool NotEnoughPlayersInGame() {
        return GameStateManager.CurrentState == GameState.Game && !PlayerManager.IsEnoughPlayersToStartGame;
    }
}
