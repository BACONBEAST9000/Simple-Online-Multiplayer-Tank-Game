using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectionMessageSender : SingletonPersistent<ConnectionMessageSender> {

    private static Dictionary<PopupMessageType, string> _messages = new Dictionary<PopupMessageType, string> {
        { PopupMessageType.LEFTGAME, "You left the game."},
        { PopupMessageType.SHUTDOWN, "The game session was shut down."}, 
        { PopupMessageType.KICKED, "The host kicked you from the lobby." },
        { PopupMessageType.GAMEISFULL, "The game you tried to join is already full!" },
        { PopupMessageType.HOSTLEFT, "The host disconnected, therefore you disconnected!" },
        { PopupMessageType.CANNOTCONNECT, "Cannot connect to game." },
        { PopupMessageType.TIMEDOUT, "Connection Timed Out! Try again later!" },
        { PopupMessageType.UNKNOWN, "Unknown error" },
    };

    [SerializeField] private RectTransform _popupUI;
    [SerializeField] private TMP_Text _messageText;

    private void OnEnable() {
        HidePopup();
        
        //MultiplayerSessionManager.OnSessionShutdown += WhenSessionShutsDown;
        MultiplayerSessionManager.OnHostShutdownSession += WhenHostShutsDownSession;
        MultiplayerSessionManager.OnClientShutdownSession += WhenClientShutsDownSession;
    }


    private void OnDisable() {
        //MultiplayerSessionManager.OnSessionShutdown -= WhenSessionShutsDown;
        MultiplayerSessionManager.OnHostShutdownSession -= WhenHostShutsDownSession;
        MultiplayerSessionManager.OnClientShutdownSession -= WhenClientShutsDownSession;
    }

    private void WhenClientShutsDownSession() {
        Show(PopupMessageType.LEFTGAME);
    }

    private void WhenHostShutsDownSession() => Show(PopupMessageType.HOSTLEFT);

    private void Show(PopupMessageType messageType) {
        if (!_messages.TryGetValue(messageType, out string message)) {
            Debug.LogError($"Didn't find a message for Message Type: ({messageType}). Returning.");
            return;
        }

        _messageText.text = message;
        ShowPopup();
    }

    private void ShowPopup() => _popupUI.gameObject.SetActive(true);
    private void HidePopup() => _popupUI.gameObject.SetActive(false);
}
