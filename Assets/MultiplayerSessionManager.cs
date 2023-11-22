using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerSessionManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft {
    private const string SESSION_NAME = "TestRoom";

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private Transform[] _playerOrderedSpawnPositions;

    private Dictionary<PlayerRef, NetworkObject> _spawnedPlayers = new();

    private NetworkRunner _runner;

    public void PlayerJoined(PlayerRef player) {
        print("Player joined");
        if (Runner.IsServer) {
            //Vector3 spawnPosition = _playerOrderedSpawnPositions[player.RawEncoded % _playerOrderedSpawnPositions.Length].position;
            Vector3 spawnPosition = transform.position;
            NetworkObject networkPlayerObject = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            _spawnedPlayers.Add(player, networkPlayerObject);
        }
    }

    public void PlayerLeft(PlayerRef player) {
        print("Player left");
        if (_spawnedPlayers.TryGetValue(player, out NetworkObject networkObject)) {
            Runner.Despawn(networkObject);
            _spawnedPlayers.Remove(player);
        }
    }


    private async void StartGame(GameMode mode) {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        StartGameArgs game = new StartGameArgs() {
            GameMode = mode,
            SessionName = SESSION_NAME,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        };
        await _runner.StartGame(game);
        print("Game Started!");
    }

    private void OnGUI() {
        HostAndClientButtons();
    }

    private void HostAndClientButtons() {
        if (_runner != null) {
            return;
        }

        if (GUI.Button(new Rect(0, 0, 200, 40), "Host")) {
            StartGame(GameMode.Host);
        }

        if (GUI.Button(new Rect(0, 50, 200, 40), "Client")) {
            StartGame(GameMode.Client);
        }
    }

    
}
