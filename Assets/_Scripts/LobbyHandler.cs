using Fusion;
using UnityEngine;

public class LobbyHandler : NetworkBehaviour {

    [SerializeField] private PlayerSpawnHandler _playerSpawner;
    [SerializeField] private NetworkTimer _lobbyTimer;

    private void OnEnable() {
        _lobbyTimer.OnTimerEnd -= WhenLobbyTimerEnds;
        _lobbyTimer.OnTimerEnd += WhenLobbyTimerEnds;
    }

    private void OnDisable() {
        _lobbyTimer.OnTimerEnd -= WhenLobbyTimerEnds;
    }

    private void WhenLobbyTimerEnds() {
        MultiplayerSessionManager.Instance.StartGame();
    }

    public override void Spawned() {
        if (GameStateManager.CurrentState == GameState.GameEnd) {
            _playerSpawner.SpawnPlayers();
        }

        GameStateManager.ChangeState(GameState.Lobby);
        
        MultiplayerSessionManager.Instance.OpenGameSession();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            MultiplayerSessionManager.Instance.ShutdownSession();
        }
    }
}
