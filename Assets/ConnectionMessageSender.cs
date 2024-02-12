using Fusion;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionMessageSender : SingletonPersistent<ConnectionMessageSender> {

    private static Dictionary<PopupMessageType, string> _messages = new Dictionary<PopupMessageType, string> {
        { new PopupMessageType(ShutdownReason.Ok), "You left the game." },
        { new PopupMessageType(ShutdownReason.Error), "There was an error. Make sure you're connected to the internet or try again later." },
        { new PopupMessageType(ShutdownReason.IncompatibleConfiguration), "Somehow, you tried to connect to a game that uses Shared Mode instead of Client-Server." },
        { new PopupMessageType(ShutdownReason.ServerInRoom), "You are already hosting a game session." },
        { new PopupMessageType(ShutdownReason.DisconnectedByPluginLogic), "The host disconnected." },
        { new PopupMessageType(ShutdownReason.GameClosed), "The game session you tried to join is closed off and not accepting players currently." },
        { new PopupMessageType(ShutdownReason.GameNotFound), "The game session you tried to join doesn't exist." },
        { new PopupMessageType(ShutdownReason.MaxCcuReached), "The maximum concurrent player number has been reached. Please wait for other players to disconnect and try again." },
        { new PopupMessageType(ShutdownReason.InvalidRegion), "The region of the session you're trying to join is unavailable or doesn't exist." },
        { new PopupMessageType(ShutdownReason.GameIdAlreadyExists), "The session with the same ID already exists." },
        { new PopupMessageType(ShutdownReason.GameIsFull), "The game session you tried to join is already full." },
        { new PopupMessageType(ShutdownReason.HostMigration), "Host Migration is taking place..." },
        { new PopupMessageType(ShutdownReason.ConnectionRefused), "Connection was refused." },
        { new PopupMessageType(ShutdownReason.ConnectionTimeout), "Connection to remote server timed out." },
        { new PopupMessageType(ShutdownReason.PhotonCloudTimeout), "Connection to Photon Cloud timed out." },
       
        { new PopupMessageType(CustomMessageType.Disconnected), "You have been disconnected from the game." },
        { new PopupMessageType(CustomMessageType.ConnectionFailed), "Failed to connect to game session" },
    };

    [SerializeField] private RectTransform _popupUI;
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Button _closeButton;

    private void OnEnable() {
        HidePopup();
        
        _closeButton.onClick.AddListener(() => {
            HidePopup();
        });

        MultiplayerSessionManager.OnSessionShutdown += WhenSessionIsShutdown;
        MultiplayerSessionManager.OnDisconnected += WhenDisconnected;
        MultiplayerSessionManager.OnConnectionFailed += WhenConnectionFailed;
    }


    private void OnDisable() {
        _closeButton.onClick.RemoveAllListeners();

        MultiplayerSessionManager.OnSessionShutdown -= WhenSessionIsShutdown;
        MultiplayerSessionManager.OnDisconnected -= WhenDisconnected;
        MultiplayerSessionManager.OnConnectionFailed -= WhenConnectionFailed;
    }

    private void WhenSessionIsShutdown(ShutdownReason reason) {
        Show(new PopupMessageType(reason));
    }

    private void WhenDisconnected(NetworkRunner runner) => Show(new PopupMessageType(CustomMessageType.Disconnected));
    private void WhenConnectionFailed(Fusion.Sockets.NetConnectFailedReason obj) => Show(new PopupMessageType(CustomMessageType.ConnectionFailed));


    private void Show(PopupMessageType messageType) {
        if (PopupAlreadyShowing()) {
            return;
        }

        string message = "";
        if (!_messages.TryGetValue(messageType, out message)) {
            message = $"An unknown error occured: {messageType.ToString()}";
        }

        ShowMessage(message);
    }

    public void ShowMessage(string message) {
        _messageText.text = message;
        ShowPopup();
    }

    private bool PopupAlreadyShowing() => _popupUI.gameObject.activeSelf;

    private void ShowPopup() => _popupUI.gameObject.SetActive(true);
    private void HidePopup() => _popupUI.gameObject.SetActive(false);
}
