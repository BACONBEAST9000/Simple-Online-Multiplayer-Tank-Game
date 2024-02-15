using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    private const string PLAYERPREFS_NICKNAME = "LocalPlayerNickname";

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
        SetupButtons();
        SetupInputFields();
    }

    private void SetupButtons() {
        _hostGameButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.StartHostSession();
            SaveEnteredNickname();
        });

        _joinGameButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.StartClientSession();
            SaveEnteredNickname();
        });

        UpdateButtonsText();
    }

    private void SetupInputFields() {
        _roomNameInputField.onValueChanged.AddListener((inputString) => {
            UpdateButtonsText();
        });

        _nicknameInputField.text = PlayerPrefs.HasKey(PLAYERPREFS_NICKNAME) ? PlayerPrefs.GetString(PLAYERPREFS_NICKNAME) : "";
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

    private void SaveEnteredNickname() {
        string playerNameEntered = _nicknameInputField.text;
        if (string.IsNullOrWhiteSpace(playerNameEntered)) {
            return;
        }

        PlayerPrefs.SetString(PLAYERPREFS_NICKNAME, playerNameEntered);
        PlayerPrefs.Save();
    }

    private void OnDestroy() {
        _hostGameButton.onClick.RemoveAllListeners();
        _joinGameButton.onClick.RemoveAllListeners();
    }
}
