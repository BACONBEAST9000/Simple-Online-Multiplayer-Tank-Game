using UnityEngine;
using UnityEngine.UI;

public class ConnectingUI : MonoBehaviour {

    [SerializeField] private RectTransform _UIElement;
    [SerializeField] private Button _cancelConnectionButton;

    private void OnEnable() {
        MultiplayerSessionManager.OnConnectingStart -= WhenConnecting;
        MultiplayerSessionManager.OnConnectingStart += WhenConnecting;

        MultiplayerSessionManager.OnConnectingEnd -= WhenConnectingEnds;
        MultiplayerSessionManager.OnConnectingEnd += WhenConnectingEnds;
        
        MultiplayerSessionManager.OnPlayerConnectedToGame -= WhenPlayerConnectsToAGame;
        MultiplayerSessionManager.OnPlayerConnectedToGame += WhenPlayerConnectsToAGame;
    }


    private void OnDisable() {
        MultiplayerSessionManager.OnPlayerConnectedToGame -= WhenPlayerConnectsToAGame;
        MultiplayerSessionManager.OnConnectingStart -= WhenConnecting;
        MultiplayerSessionManager.OnConnectingEnd -= WhenConnectingEnds;
    }

    private void Awake() {
        _cancelConnectionButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.ShutdownSession();
        });
    }

    private void OnDestroy() {
        _cancelConnectionButton.onClick.RemoveAllListeners();
    }

    private void WhenConnecting() {
        Show();
    }
    
    private void WhenConnectingEnds() {
        Hide();
    }

    private void WhenPlayerConnectsToAGame() {
        Hide();
    }

    public void Show() => _UIElement.gameObject.SetActive(true);
    public void Hide() => _UIElement.gameObject.SetActive(false);
}
