using Fusion;
using UnityEngine;

public class SpawnPlayersIfLobbyAndCameFromGame : NetworkBehaviour {

    [SerializeField] private PlayerSpawnHandler _spawnHandler;

    public override void Spawned() {
        if (GameStateManager.PreviousState == GameState.GameEnd && GameStateManager.CurrentState == GameState.Lobby) {
            _spawnHandler.SpawnPlayers();
        }
    }
}
