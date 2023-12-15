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
        if (Object.HasStateAuthority == false) return;

        int positionIndex = 0;
        foreach (PlayerRef playerRef in Runner.ActivePlayers) {
            Player player = MultiplayerSessionManager.Instance.SpawnPlayer(playerRef);
            // TODO: This is a duplicate instance of assigning player's spawn position and rotation!
            player.transform.position = _spawnPoints[positionIndex].position;
            player.transform.rotation = _spawnPoints[positionIndex].rotation;

            positionIndex++;
        }
    }
}
