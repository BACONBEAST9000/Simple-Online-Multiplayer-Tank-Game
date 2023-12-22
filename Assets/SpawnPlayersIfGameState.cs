using Fusion;
using UnityEngine;

public class SpawnPlayersIfGameState : NetworkBehaviour {

    [SerializeField] private GameState _stateWhenShouldSpawn;
    [SerializeField] private PlayerSpawnHandler _spawnHandler;

    public override void Spawned() {
        if (GameStateManager.CurrentState == _stateWhenShouldSpawn) {
            _spawnHandler.SpawnPlayers();
        }
    }
}
