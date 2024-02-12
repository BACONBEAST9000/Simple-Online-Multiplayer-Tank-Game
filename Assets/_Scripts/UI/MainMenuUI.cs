using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    
    [Header("Host/Join Buttons")]
    [SerializeField] private Button _hostGameButton;
    [SerializeField] private Button _joinGameButton;
    
    [Header("Host/Join Button Text")]
    [SerializeField] private TMP_Text _hostGameButtonText;
    [SerializeField] private TMP_Text _joinGameButtonText;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField _nicknameInputField;
    [SerializeField] private TMP_InputField _roomNameInputField;

    public TMP_InputField GetNicknameInputField => _nicknameInputField;
    public TMP_InputField GetRoomNameInputField => _roomNameInputField;

    private void Awake() {
        _hostGameButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.StartHostSession();
        });
        
        _joinGameButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.StartClientSession();
        });

        UpdateButtonsText();

        _roomNameInputField.onValueChanged.AddListener((inputString) => {
            UpdateButtonsText();
        });
    }

    private void UpdateButtonsText() {
        UpdateHostButtonText();
        UpdateJoinButtonText();
    }

    private void UpdateHostButtonText() {
        string textIfEmpty = "Host Game";
        string textIfNonEmpty = $"Host Game with Room Name: \"{_roomNameInputField.text}\"";
        string roomName = GetRoomNameInputField.text;

        SetTextBasedOnInputEmptyOrNot(roomName, _hostGameButtonText, textIfEmpty, textIfNonEmpty);
    }
    
    private void UpdateJoinButtonText() {
        string textIfEmpty = "Join any available game";
        string textIfNonEmpty = $"Join room: \"{_roomNameInputField.text}\"";
        string roomName = GetRoomNameInputField.text;

        SetTextBasedOnInputEmptyOrNot(roomName, _joinGameButtonText, textIfEmpty, textIfNonEmpty);
    }

    private void SetTextBasedOnInputEmptyOrNot(string input, TMP_Text textElement, string textIfEmpty, string textIfNotEmpty) {
        textElement.text = string.IsNullOrWhiteSpace(input) ? textIfEmpty : textIfNotEmpty;
    }

    private void OnDestroy() {
        _hostGameButton.onClick.RemoveAllListeners();
        _joinGameButton.onClick.RemoveAllListeners();
    }
}
