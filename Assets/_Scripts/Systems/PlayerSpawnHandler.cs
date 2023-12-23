using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnHandler : NetworkBehaviour {

    [SerializeField] private bool _spawnPlayersOnSpawned = true;
    [SerializeField] private List<Transform> _spawnPoints;

    public override void Spawned() {
        if (_spawnPlayersOnSpawned || CurrentlyLobbyAfterGameEnded) {
            SpawnPlayers();
        }
        print("Just came back from a game: " + CurrentlyLobbyAfterGameEnded);
    }

    private bool CurrentlyLobbyAfterGameEnded => GameStateManager.PreviousState == GameState.GameEnd && GameStateManager.CurrentState == GameState.Lobby;

    public void SpawnPlayers() {
        if (Object.HasStateAuthority == false) return;

        int positionIndex = 0;
        foreach (PlayerRef playerRef in Runner.ActivePlayers) {
            Player player = MultiplayerSessionManager.Instance.SpawnPlayer(playerRef);

            SetPlayerToTransform(_spawnPoints[positionIndex], player);

            positionIndex++;
        }
    }

    public static void SetPlayerToTransform(Transform transformPoint, Player player) {
        player.transform.position = transformPoint.position;
        player.transform.rotation = transformPoint.rotation;
    }
}
