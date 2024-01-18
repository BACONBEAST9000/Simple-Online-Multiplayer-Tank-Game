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

        foreach (PlayerRef playerRef in Runner.ActivePlayers) {
            Player player = MultiplayerSessionManager.Instance.SpawnPlayer(playerRef);
            SetPlayerToSpawnPoint(player);
        }
    }

    public void SetPlayerToSpawnPoint(Player player) {
        Transform respawnPoint = Utils.GetFarthestTransformFromPlayers(_spawnPoints);
        SetPlayerToTransform(respawnPoint, player);
    }

    public static void SetPlayerToTransform(Transform transformPoint, Player player) {
        player.transform.position = transformPoint.position;
        player.transform.rotation = transformPoint.rotation;
    }
}
