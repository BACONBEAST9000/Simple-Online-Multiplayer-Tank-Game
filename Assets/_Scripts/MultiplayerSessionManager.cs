using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerSessionManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft, INetworkRunnerCallbacks {
    public static MultiplayerSessionManager Instance;
    
    private const string SESSION_NAME = "TestRoom";
    private const string GAME_SCENE_NAME = "MainGame";

    public static event Action OnPlayerJoinedGame;
    public static event Action OnPlayerConnectedToGame;

    [SerializeField] private Player _playerPrefab;
    [SerializeField] private PlayerData _playerDataPrefab;
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private Transform[] _playerOrderedSpawnPositions;

    private Dictionary<PlayerRef, Player> _spawnedPlayers = new();
    public Dictionary<PlayerRef, Player> SpawnedPlayers => _spawnedPlayers;

    private NetworkRunner _runner;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            print("Players in lobby: " + Runner.ActivePlayers);
            foreach (PlayerRef player in Runner.ActivePlayers) {

                if (!Runner.TryGetPlayerObject(player, out var playerObject)) {
                    continue;
                }

                if (!playerObject.TryGetComponent(out Player p)) {
                    continue;
                }

                print(p.NickName);
            }
        }

    }

    private void UpdatePlayerData() {
        var playerData = FindObjectOfType<PlayerData>();
        if (playerData == null) {
            playerData = Instantiate(_playerDataPrefab);
        }

        if (string.IsNullOrWhiteSpace(_nameInputField.text)) {
            playerData.NickName = PlayerData.GetRandomNickName();
        }
        else {
            playerData.NickName = _nameInputField.text;
        }
    }

    public void PlayerJoined(PlayerRef playerRef) {
        print("Player joined");
        if (Runner.IsServer) {
            Vector3 spawnPosition = _playerOrderedSpawnPositions[playerRef.RawEncoded % _playerOrderedSpawnPositions.Length].position;
            Player newPlayer = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, playerRef);

            Runner.SetPlayerObject(playerRef, newPlayer.Object);

            _spawnedPlayers.Add(playerRef, newPlayer);
            OnPlayerJoinedGame?.Invoke();
        }
    }

    public void PlayerLeft(PlayerRef player) {
        print("Player left");
        if (_spawnedPlayers.TryGetValue(player, out Player playerThatLeft)) {
            Runner.Despawn(playerThatLeft.Object);
            _spawnedPlayers.Remove(player);
        }
    }


    private async void StartGame(GameMode mode) {
        UpdatePlayerData();

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
        
        //_runner.SetActiveScene(GAME_SCENE_NAME);
    }

    public void StartHostGame() => StartGame(GameMode.Host);
    public void StartClientGame() => StartGame(GameMode.Client);

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        print("Players in lobby: " + SpawnedPlayers.Count);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
        
    }

    public void OnConnectedToServer(NetworkRunner runner) {
        print("Connected to server");
        OnPlayerConnectedToGame?.Invoke();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner) {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {
        print("Connect request");
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
