using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private RectTransform _UIElement;
    [SerializeField] private Button _hostGameButton;
    [SerializeField] private Button _joinGameButton;

    private void OnEnable() {
        MultiplayerSessionManager.OnConnectingStart -= WhenConnecting;
        MultiplayerSessionManager.OnConnectingStart += WhenConnecting;

        MultiplayerSessionManager.OnSessionShutdown -= WhenSessionShutdown;
        MultiplayerSessionManager.OnSessionShutdown += WhenSessionShutdown;
    }

    private void OnDisable() {
        MultiplayerSessionManager.OnConnectingStart -= WhenConnecting;
        MultiplayerSessionManager.OnSessionShutdown -= WhenSessionShutdown;
    }

    private void WhenConnecting() {
        Hide();
    }
    
    private void WhenSessionShutdown() {
        Show();
    }

    private void Awake() {
        _hostGameButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.StartHostSession();
        });
        
        _joinGameButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.StartClientSession();
        });
    }

    private void OnDestroy() {
        _hostGameButton.onClick.RemoveAllListeners();
        _joinGameButton.onClick.RemoveAllListeners();
    }

    public void Show() => _UIElement.gameObject.SetActive(true);
    public void Hide() => _UIElement.gameObject.SetActive(false);
}
