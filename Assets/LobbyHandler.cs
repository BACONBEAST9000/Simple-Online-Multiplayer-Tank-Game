using Fusion;
using UnityEngine;

public class LobbyHandler : NetworkBehaviour {

    [SerializeField] private PlayerSpawnHandler _playerSpawner;

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
