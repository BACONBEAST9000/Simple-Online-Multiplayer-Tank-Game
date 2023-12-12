using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnHandler : NetworkBehaviour {

    [SerializeField] private bool _spawnPlayersOnSpawned = true;
    [SerializeField] private List<Transform> _spawnPoints;

    public override void Spawned() {
        if (_spawnPlayersOnSpawned) {
            SpawnPlayers();
        }
    }

    public void SpawnPlayers() {
        int positionIndex = 0;
        foreach (PlayerRef playerRef in Runner.ActivePlayers) {
            Player player = MultiplayerSessionManager.Instance.SpawnPlayer(playerRef);
            player.transform.position = _spawnPoints[positionIndex].position;
            positionIndex++;
        }
    }
}
