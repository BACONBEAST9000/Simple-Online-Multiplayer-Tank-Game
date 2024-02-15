using TMPro;
using UnityEngine;

public class DebugTesting : SingletonNetworkPersistent<DebugTesting> {

    [Header("Properties for Testing")]
    
    [Tooltip("Enabling Test Mode displays test stats and allows for additional functionality related to testing features.")]
    [SerializeField] private bool _testModeEnabled;

    [Tooltip("Enabling this allows the host to play the game without the need for other players to be in the lobby.")]
    [SerializeField] private bool _allowSoloPlay;

    [Header("Requirements")]
    [SerializeField] private DebugManager _debugManager;
    [SerializeField] private TMP_Text _gameStateText;

    public bool TestModeEnabled => _testModeEnabled;

    private ReadyUpManager _readyUpManager;

    public override void Awake() {
        base.Awake();

        _debugManager.gameObject.SetActive(_testModeEnabled);

        if (_testModeEnabled) {
            UpdateCurrentStateText(GameStateManager.CurrentState);
        }

        if (_allowSoloPlay) {
            UpdateSoloPlayAllowed();
        }
    }

    private void UpdateSoloPlayAllowed() {
        if (_readyUpManager == null) {
            _readyUpManager = FindObjectOfType<ReadyUpManager>();
        }

        if (_readyUpManager != null) {
            _readyUpManager.AllowForSoloPlay = _allowSoloPlay;
        }
    }

    private void OnEnable() {
        GameStateManager.OnStateChanged -= WhenStateChanges;
        GameStateManager.OnStateChanged += WhenStateChanges;
    }

    private void OnDisable() {
        GameStateManager.OnStateChanged -= WhenStateChanges;
    }

    private void WhenStateChanges(GameState newState) {
        UpdateCurrentStateText(newState);
    }

    private void UpdateCurrentStateText(GameState newState) {
        _gameStateText.text = "Game State: " + newState.ToString();
    } 

    private void Update() {
        if (!_testModeEnabled)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (GameStateManager.CurrentState == GameState.Game) {
                GameHandler gameManagerInstance = FindObjectOfType<GameHandler>();

                gameManagerInstance?.RPC_EndGame();
            }
        }
    }
}
