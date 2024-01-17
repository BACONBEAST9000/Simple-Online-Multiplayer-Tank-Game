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
    private const string MENU_SCENE_NAME = "MainMenu";
    private const string GAME_SCENE_NAME = "MainGame";

    public static event Action OnPlayerJoinedGame;
    public static event Action OnPlayerConnectedToGame;
    public static event Action OnConnectingStart;
    public static event Action OnConnectingEnd;
    public static event Action OnSessionShutdown;
    public static event Action OnSceneStartedLoading;
    public static event Action OnSceneLoaded;

    [SerializeField] private Player _playerPrefab;
    [SerializeField] private LocalPlayerData _playerDataPrefab;
    [SerializeField] private TMP_InputField _nameInputField;
    
    // TODO: Remove
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
            Destroy(gameObject);
        }
    }

    private void UpdatePlayerData() {
        var playerData = FindObjectOfType<LocalPlayerData>();
        if (playerData == null) {
            playerData = Instantiate(_playerDataPrefab);
        }

        if (string.IsNullOrWhiteSpace(_nameInputField.text)) {
            playerData.NickName = LocalPlayerData.GetRandomNickName();
        }
        else {
            playerData.NickName = _nameInputField.text;
        }
    }

    public void PlayerJoined(PlayerRef playerRef) {
        print("Player joined");
        
        if (Runner.IsServer) {
            Player newPlayer = SpawnPlayer(playerRef);

            Transform spawnPoint = _playerOrderedSpawnPositions[playerRef.RawEncoded % _playerOrderedSpawnPositions.Length];

            PlayerSpawnHandler.SetPlayerToTransform(spawnPoint, newPlayer);

            OnPlayerJoinedGame?.Invoke();
        }
    }

    public Player SpawnPlayer(PlayerRef playerRef) {
        Player newPlayer = Runner.Spawn(_playerPrefab, transform.position, Quaternion.identity, playerRef);

        Runner.SetPlayerObject(playerRef, newPlayer.Object);

        PlayerManager.UpdatePlayerWithReference(playerRef, newPlayer);

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

        if (_runner && _runner.IsRunning) {
            print("Session Started and Is RUNNING!");
        }
    }

    public void StartHostSession() => StartSession(GameMode.Host);
    public void StartClientSession() => StartSession(GameMode.Client);

    public void ShutdownSession() => _runner.Shutdown();

    public void KickPlayer(Player player) => _runner.Disconnect(player.PlayerID); 

    public void StartGame() {
        CloseGameSession();
        GameStateManager.ChangeState(GameState.PreGameStart);
        LoadGameScene();
    }

    private void SetGameSessionOpen(bool isOpenToJoin) {
        Runner.SessionInfo.IsOpen = isOpenToJoin;
        Runner.SessionInfo.IsVisible = isOpenToJoin;
    }

    public void OpenGameSession() => SetGameSessionOpen(true);
    public void CloseGameSession() => SetGameSessionOpen(false);

    public void LoadMenuScene() => LoadScene(MENU_SCENE_NAME);
    public void LoadGameScene() => LoadScene(GAME_SCENE_NAME);

    private void LoadScene(string sceneName) => Runner.SetActiveScene(sceneName);

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
        Shutdown();
    }

    private void Shutdown() {
        GameStateManager.ChangeState(GameState.Menu);
        SceneManager.LoadScene(0);
        OnSessionShutdown?.Invoke();
        Destroy(gameObject);
    }

    public void OnConnectedToServer(NetworkRunner runner) {
        print("Connected to server");
        OnPlayerConnectedToGame?.Invoke();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner) {
        print("Disconnected from server");

        Shutdown();
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
        OnSceneLoaded?.Invoke();
    }

    public void OnSceneLoadStart(NetworkRunner runner) {
        print("Scene Load Start");
        OnSceneStartedLoading?.Invoke();
    }
}
