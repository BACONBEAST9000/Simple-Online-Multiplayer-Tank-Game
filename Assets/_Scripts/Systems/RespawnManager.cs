using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RespawnManager : SingletonNetwork<RespawnManager> {

    public static event Action<Player> OnRespawnedPlayer;

    [SerializeField] private List<Transform> _spawnPoints;

    public void Respawn(Player player) {
        Transform respawnPoint = (PlayerManager.GetPlayerCount < 2)
            ? GetRandomSpawnPoint()
            : Utils.GetFarthestTransformFromPlayers(_spawnPoints);

        PlayerSpawnHandler.SetPlayerToTransform(respawnPoint, player);
        OnRespawnedPlayer?.Invoke(player);
    }

    private Transform GetRandomSpawnPoint() {
        return _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)];
    }
}
