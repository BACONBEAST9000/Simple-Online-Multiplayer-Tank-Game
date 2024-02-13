using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Refactor
public class MultiplayerSessionManager : SingletonSimulationNetwork<MultiplayerSessionManager>, IPlayerJoined, IPlayerLeft, INetworkRunnerCallbacks {
    
    private const string SESSION_NAME = "TestRoom";
    private const string MENU_SCENE_NAME = "MainMenu";
    private const string GAME_SCENE_NAME = "MainGame";

    public static event Action OnPlayerJoinedGame;
    public static event Action OnPlayerLeftGame;
    public static event Action OnPlayerConnectedToGame;
    public static event Action OnConnectingStart;
    public static event Action OnConnectingEnd;
    public static event Action<ShutdownReason> OnSessionShutdown;
    public static event Action OnSceneStartedLoading;
    public static event Action OnSceneLoaded;
    public static event Action<NetConnectFailedReason> OnConnectionFailed;

    public static event Action<NetworkRunner> OnDisconnected;
    public static event Action OnPlayerKicked;

    [SerializeField] private Player _playerPrefab;
    [SerializeField] private LocalPlayerData _playerDataPrefab;
    // Still probabily need alternative...
    [SerializeField] private MainMenuUI _mainMenuUI;

    private NetworkRunner _runner;

    private PlayerSpawnHandler _spawnHandler;

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

    public SessionInfo GetSessionInfo() {
        if (_runner == null) {
            return null;
        }

        return _runner.SessionInfo;
    }

    // TODO: Refactor
    private void UpdatePlayerData() {
        LocalPlayerData playerData = FindObjectOfType<LocalPlayerData>();
        if (playerData == null) {
            playerData = Instantiate(_playerDataPrefab);
        }

        if (string.IsNullOrWhiteSpace(_mainMenuUI.GetNicknameInputField.text)) {
            playerData.NickName = LocalPlayerData.GetRandomNickName();
        }
        else {
            playerData.NickName = _mainMenuUI.GetNicknameInputField.text;
        }
    }

    public void PlayerJoined(PlayerRef playerRef) {
        print("Player joined");
        
        if (Runner.IsServer) {
            Player newPlayer = SpawnPlayer(playerRef);

            if (_spawnHandler == null) {
                _spawnHandler = FindObjectOfType<PlayerSpawnHandler>();
            }

            _spawnHandler?.SetPlayerToSpawnPoint(newPlayer);

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

        OnPlayerLeftGame?.Invoke();
    }

    private async void StartSession(GameMode mode) {
        UpdatePlayerData();

        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        StartGameArgs game = new StartGameArgs() {
            PlayerCount = 4,
            GameMode = mode,
            SessionName = _mainMenuUI.GetRoomNameInputField.text,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        };

        OnConnectingStart?.Invoke();
        await _runner.StartGame(game);
        OnConnectingEnd?.Invoke();
    }

    public void StartHostSession() => StartSession(GameMode.Host);
    public void StartClientSession() => StartSession(GameMode.Client);

    public void ShutdownSession() {
        if (_runner == null) return;

        _runner.Shutdown();
    }

    public void KickPlayer(Player player) {
        RPC_Kick(_runner, player.PlayerID);
        OnPlayerKicked?.Invoke();
        _runner.Disconnect(player.PlayerID);
    }

    [Rpc]
    private static void RPC_Kick(NetworkRunner runner, [RpcTarget] PlayerRef playerToKick) {
        print($"[{playerToKick}] YOU GOT KICKED!");
    }

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


    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
        print("Shutdown! The reason message: " + shutdownReason);        
        Shutdown(shutdownReason);
    }

    private void Shutdown(ShutdownReason reason = ShutdownReason.Ok) {
        GameStateManager.ChangeState(GameState.Menu);
        SceneManager.LoadScene(0);
        OnSessionShutdown?.Invoke(reason);
        Destroy(gameObject);
    }

    public void OnConnectedToServer(NetworkRunner runner) {
        print("Connected to server");
        OnPlayerConnectedToGame?.Invoke();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner) {
        print("Disconnected from server");
        OnDisconnected?.Invoke(runner);
        Shutdown();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {
        print("Connect request");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {
        print("Failed to connect! " + reason);
        OnConnectionFailed?.Invoke(reason);
    }
    public void OnSceneLoadDone(NetworkRunner runner) {
        print("Scene Load Done!");
        OnSceneLoaded?.Invoke();
    }

    public void OnSceneLoadStart(NetworkRunner runner) {
        print("Scene Load Start");
        OnSceneStartedLoading?.Invoke();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        print($"Player {player} has joined!");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        print($"Player {player} has left!");
    }

    #region Unused Functions from INetworkRunnerCallbacks

    public void OnInput(NetworkRunner runner, NetworkInput input) {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {
        
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
    #endregion
}
