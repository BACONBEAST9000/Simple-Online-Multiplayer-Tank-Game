using Fusion;
using UnityEngine;

public class LobbyHandler : NetworkBehaviour {
    private const int SECONDS_UNTIL_GAME_START = 180;

    [SerializeField] private PlayerSpawnHandler _playerSpawner;
    [SerializeField] private NetworkTimer _lobbyTimer;
    [SerializeField] private RectTransform _timeLeftUI;
    [SerializeField] private RectTransform _waitingForPlayersUI;

    private bool _notEnoughPlayersBeforeNow = true;

    private void OnEnable() {
        _lobbyTimer.OnTimerEnd -= WhenLobbyTimerEnds;
        _lobbyTimer.OnTimerEnd += WhenLobbyTimerEnds;

        PlayerManager.OnPlayerListUpdated -= WhenPlayerListUpdated;
        PlayerManager.OnPlayerListUpdated += WhenPlayerListUpdated;  
    }


    private void OnDisable() {
        _lobbyTimer.OnTimerEnd -= WhenLobbyTimerEnds;
        PlayerManager.OnPlayerListUpdated -= WhenPlayerListUpdated;
    }
    
    private void WhenPlayerListUpdated() {
        CheckIfUpdateLobbyTimerAndUI();
    }
    
    private void CheckIfUpdateLobbyTimerAndUI() {
        if (JustGotEnoughPlayersToPlay()) {
            _lobbyTimer.StartTimer(SECONDS_UNTIL_GAME_START);
            
            ShowTimeLeftUI();
            HideWaitingForPlayersUI();
            
            _notEnoughPlayersBeforeNow = false;
        }
        else if (!PlayerManager.IsEnoughPlayersToStartGame) {
            _lobbyTimer.StopTimer();
            
            HideTimeLeftUI();
            ShowWaitingForPlayersUI();
            
            _notEnoughPlayersBeforeNow = true;
        }
    }

    private bool JustGotEnoughPlayersToPlay() => _notEnoughPlayersBeforeNow && PlayerManager.IsEnoughPlayersToStartGame;

    private void ShowWaitingForPlayersUI() => _waitingForPlayersUI?.gameObject?.SetActive(true);
    private void HideWaitingForPlayersUI() => _waitingForPlayersUI?.gameObject?.SetActive(false);

    private void ShowTimeLeftUI() => _timeLeftUI?.gameObject?.SetActive(true);
    private void HideTimeLeftUI() => _timeLeftUI?.gameObject?.SetActive(false);


    private void WhenLobbyTimerEnds() {
        MultiplayerSessionManager.Instance.StartGame();
    }

    public override void Spawned() {
        if (GameStateManager.CurrentState == GameState.GameEnd) {
            _playerSpawner.SpawnPlayers();
        }

        GameStateManager.ChangeState(GameState.Lobby);
        
        MultiplayerSessionManager.Instance.OpenGameSession();

        CheckIfUpdateLobbyTimerAndUI();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            MultiplayerSessionManager.Instance.ShutdownSession();
        }
    }
}
