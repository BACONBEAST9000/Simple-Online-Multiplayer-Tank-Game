using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Refactor
public class MultiplayerSessionManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft, INetworkRunnerCallbacks {
    public static MultiplayerSessionManager Instance;
    
    private const string SESSION_NAME = "TestRoom";
    private const string GAME_SCENE_NAME = "MainGame";

    public static event Action OnPlayerJoinedGame;
    public static event Action OnPlayerConnectedToGame;
    public static event Action OnConnectingStart;
    public static event Action OnConnectingEnd;
    public static event Action OnSessionShutdown;

    [SerializeField] private Player _playerPrefab;
    [SerializeField] private PlayerData _playerDataPrefab;
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private Transform[] _playerOrderedSpawnPositions;

    private NetworkRunner _runner;

    private void OnEnable() {
        ReadyUpManager.OnAllPlayersReady -= WhenAllPlayersAreReady;
        ReadyUpManager.OnAllPlayersReady += WhenAllPlayersAreReady;
    }

    private void OnDisable() {
        ReadyUpManager.OnAllPlayersReady -= WhenAllPlayersAreReady;
    }
    
    private void WhenAllPlayersAreReady() {
        StartGame();
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
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
            Player newPlayer = SpawnPlayer(playerRef);

            Vector3 spawnPosition = _playerOrderedSpawnPositions[playerRef.RawEncoded % _playerOrderedSpawnPositions.Length].position;
            newPlayer.transform.position = spawnPosition;
            OnPlayerJoinedGame?.Invoke();
        }
    }

    public Player SpawnPlayer(PlayerRef playerRef) {
        Player newPlayer = Runner.Spawn(_playerPrefab, transform.position, Quaternion.identity, playerRef);

        print($"Spawn Player: {playerRef}, new player object: {((newPlayer && newPlayer.Object != null) ? newPlayer.Object : "newPlayer.Object is null")}");

        Runner.SetPlayerObject(playerRef, newPlayer.Object);

        PlayerManager.UpdatePlayerWithReference(playerRef, newPlayer);

        print($"Spawned Player - {playerRef} : {newPlayer}");
        return newPlayer;
    }

    public void PlayerLeft(PlayerRef playerRef) {
        print("Player left");

        Player playerThatLeftGame = PlayerManager.GetPlayerWithReference(playerRef);

        if (playerThatLeftGame) {
            Runner.Despawn(playerThatLeftGame.Object);
        }
    }


    private async void StartSession(GameMode mode) {
        UpdatePlayerData();

        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        StartGameArgs game = new StartGameArgs() {
            PlayerCount = 4,
            GameMode = mode,
            SessionName = SESSION_NAME,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        };

        OnConnectingStart?.Invoke();
        await _runner.StartGame(game);
        OnConnectingEnd?.Invoke();
    }

    public void StartHostSession() => StartSession(GameMode.Host);
    public void StartClientSession() => StartSession(GameMode.Client);

    public void ShutdownSession() => _runner.Shutdown();

    public void StartGame() {
        CloseGameSession();
        LoadGameScene();
    }

    private void CloseGameSession() {
        Runner.SessionInfo.IsOpen = false;
        Runner.SessionInfo.IsVisible = false;
    }

    private void LoadGameScene() => Runner.SetActiveScene(GAME_SCENE_NAME);

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
        print("Shutdown! The reason message: " + shutdownReason);
        SceneManager.LoadScene(0);
        OnSessionShutdown?.Invoke();
    }

    public void OnConnectedToServer(NetworkRunner runner) {
        print("Connected to server");
        OnPlayerConnectedToGame?.Invoke();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner) {
        print("Disconnected from server");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {
        print("Connect request");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {
        print("Failed to connect! " + reason);
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
        print("Scene Load Done!");
    }

    public void OnSceneLoadStart(NetworkRunner runner) {
        print("Scene Load Start");
    }
}
