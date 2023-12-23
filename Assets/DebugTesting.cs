using Fusion;
using TMPro;
using UnityEngine;

public class DebugTesting : NetworkBehaviour {

    private static DebugTesting _privateInstance;

    [SerializeField] private bool _testModeEnabled;
    [SerializeField] private TMP_Text _gameStateText;

    private void Awake() {
        if (_privateInstance == null) {
            _privateInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
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
        _gameStateText.text = "Game State: " + newState.ToString();
    }

    private void Update() {
        if (!_testModeEnabled)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (GameStateManager.CurrentState == GameState.Game) {
                GameManager gameManagerInstance = FindObjectOfType<GameManager>();

                gameManagerInstance?.RPC_EndGame();
            }
        }
    }
}
