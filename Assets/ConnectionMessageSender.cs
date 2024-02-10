using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionMessageSender : SingletonPersistent<ConnectionMessageSender> {

    private static Dictionary<PopupMessageType, string> _messages = new Dictionary<PopupMessageType, string> {
        { PopupMessageType.LEFTGAME, "You left the game."},
        { PopupMessageType.SHUTDOWN, "The game session was shut down."}, 
        { PopupMessageType.KICKED, "The host kicked you from the lobby." },
        { PopupMessageType.GAMEISFULL, "The game you tried to join is already full!" },
        { PopupMessageType.HOSTLEFT, "The host disconnected, therefore you disconnected!" },
        { PopupMessageType.CANNOTCONNECT, "Cannot connect to game." },
        { PopupMessageType.TIMEDOUT, "Connection Timed Out! Try again later!" },
        { PopupMessageType.DISCONNECTED, "You were disconnected from the game session!" },
        { PopupMessageType.CONNECTIONFAILED, "Failed to connect to game session." },
        { PopupMessageType.UNKNOWN, "Unknown error" },
    };

    [SerializeField] private RectTransform _popupUI;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Button _closeButton;

    private void OnEnable() {
        HidePopup();
        _closeButton.onClick.AddListener(() => {
            HidePopup();
        });

        MultiplayerSessionManager.OnHostShutdownSession += WhenHostShutsDownSession;
        MultiplayerSessionManager.OnClientShutdownSession += WhenClientShutsDownSession;
        MultiplayerSessionManager.OnDisconnected += WhenDisconnected;
        MultiplayerSessionManager.OnConnectionFailed += WhenConnectionFailed;
    }


    private void OnDisable() {
        _closeButton.onClick.RemoveAllListeners();
        
        MultiplayerSessionManager.OnHostShutdownSession -= WhenHostShutsDownSession;
        MultiplayerSessionManager.OnClientShutdownSession -= WhenClientShutsDownSession;
        MultiplayerSessionManager.OnDisconnected -= WhenDisconnected;
        MultiplayerSessionManager.OnConnectionFailed -= WhenConnectionFailed;
    }

    private void WhenClientShutsDownSession() => Show(PopupMessageType.LEFTGAME);
    private void WhenDisconnected(Fusion.NetworkRunner runner) => Show(PopupMessageType.DISCONNECTED);
    private void WhenConnectionFailed(Fusion.Sockets.NetConnectFailedReason obj) => Show(PopupMessageType.CONNECTIONFAILED);

    private void WhenHostShutsDownSession() => Show(PopupMessageType.HOSTLEFT);

    private void Show(PopupMessageType messageType) {
        if (PopupAlreadyShowing()) {
            return;
        }

        if (!_messages.TryGetValue(messageType, out string message)) {
            Debug.LogError($"Didn't find a message for Message Type: ({messageType}). Returning.");
            return;
        }

        _messageText.text = message;
        ShowPopup();
    }

    private bool PopupAlreadyShowing() => _popupUI.gameObject.activeSelf;

    private void ShowPopup() => _popupUI.gameObject.SetActive(true);
    private void HidePopup() => _popupUI.gameObject.SetActive(false);
}
