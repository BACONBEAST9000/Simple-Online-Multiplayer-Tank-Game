using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerSessionManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft, INetworkRunnerCallbacks {
    private const string SESSION_NAME = "TestRoom";

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private Transform[] _playerOrderedSpawnPositions;

    private Dictionary<PlayerRef, NetworkObject> _spawnedPlayers = new();

    private NetworkRunner _runner;

    private Vector2 _moveInput;
    private bool _shootInput;

    private void Update() {
        _shootInput = _shootInput | Input.GetKeyDown(KeyCode.Space);
    }

    public void PlayerJoined(PlayerRef player) {
        print("Player joined");
        if (Runner.IsServer) {
            Vector3 spawnPosition = _playerOrderedSpawnPositions[player.RawEncoded % _playerOrderedSpawnPositions.Length].position;
            //Vector3 spawnPosition = transform.position;
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

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) {
        PlayerInput playerInputData = new PlayerInput();

        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        playerInputData.MoveInput = _moveInput;

        if (_shootInput) {
            playerInputData.Buttons |= PlayerInput.SHOOT_INPUT;
        }

        _shootInput = false;

        input.Set(playerInputData);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
        
    }

    public void OnConnectedToServer(NetworkRunner runner) {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner) {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner) {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner) {
        
    }
}
