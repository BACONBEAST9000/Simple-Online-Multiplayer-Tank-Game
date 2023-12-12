using UnityEngine;

public class ConnectingUI : MonoBehaviour {

    [SerializeField] private RectTransform _UIElement;

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
