using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RespawnManager : NetworkBehaviour {
    
    public static RespawnManager Instance;

    [SerializeField] private List<Transform> _spawnPoints;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    public void Respawn(Player player) {
        Transform respawnPoint = (Runner.ActivePlayers.ToList().Count < 2)
            ? _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)]
            : GetFarthestSpawnPoint(player);

        // TODO: This is a duplicate instance of assigning player's spawn position and rotation!
        player.transform.position = respawnPoint.position;
        player.transform.rotation = respawnPoint.rotation;
    }

    public Transform GetFarthestSpawnPoint(Player player) {
        List<Vector3> playerPositions = new List<Vector3>();
        
        foreach (PlayerRef playerRef in Runner.ActivePlayers) {

            if (!Runner.TryGetPlayerObject(playerRef, out var playerObject)) {
                continue;
            }

            if (!playerObject.TryGetComponent(out Player playerComponent)) {
                continue;
            }

            if (playerComponent == player) {
                continue;
            }

            playerPositions.Add(playerComponent.transform.position);
        }

        Transform farthestSpawnPoint = null;
        float farthestDistance = float.MinValue;

        foreach (Transform spawnPoint in _spawnPoints) {

            float totalDistance = 0f;

            foreach (Vector3 playerPosition in playerPositions) {
                totalDistance += Vector3.Distance(playerPosition, spawnPoint.position);
            }

            if (totalDistance > farthestDistance) {
                farthestDistance = totalDistance;
                farthestSpawnPoint = spawnPoint;
            }
        }
        return farthestSpawnPoint;
    }
}
