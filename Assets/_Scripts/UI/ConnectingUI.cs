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

        HideUI();
    }

    private void OnDestroy() {
        MultiplayerSessionManager.OnPlayerConnectedToGame -= WhenPlayerConnectsToAGame;
        MultiplayerSessionManager.OnConnectingStart -= WhenConnecting;
        MultiplayerSessionManager.OnConnectingEnd -= WhenConnectingEnds;
        
        _cancelConnectionButton.onClick.RemoveAllListeners();
    }


    private void WhenConnecting() => ShowUI();

    private void WhenConnectingEnds() => HideUI();

    private void WhenPlayerConnectsToAGame() => HideUI();


    public void ShowUI() => _UIElement.gameObject.SetActive(true);
    public void HideUI() => _UIElement.gameObject.SetActive(false);
}
