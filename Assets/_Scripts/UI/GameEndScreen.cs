using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScreen : NetworkBehaviour {
    [SerializeField] private RectTransform _waitingText;
    [SerializeField] private RectTransform _resultsUI;
    [SerializeField] private RectTransform _allPlayersLeftUI;

    [Header("Buttons")]
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _quitButton;

    private void OnEnable() {
        if (GameStateManager.CurrentState != GameState.GameEnd) {
            HideEndScreenUI();
            return;
        }

        DisplayContent();
        ShowContinueButtonOrWaitingText();
        AddListenersToButtons();
    }

    private void DisplayContent() {
        bool enoughPlayersInGame = PlayerManager.IsEnoughPlayersToStartGame;

        _resultsUI.gameObject.SetActive(enoughPlayersInGame);
        _allPlayersLeftUI.gameObject.SetActive(!enoughPlayersInGame);
    }

    private void OnDisable() {
        RemoveListenersFromButtons();
    }

    private void AddListenersToButtons() {
        _continueButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.LoadMenuScene();
        });

        _quitButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.ShutdownSession();
        });
    }

    private void RemoveListenersFromButtons() {
        _continueButton?.onClick.RemoveAllListeners();
        _quitButton?.onClick.RemoveAllListeners();
    }

    private void ShowContinueButtonOrWaitingText() {
        SetContinueButtonActive(Object.HasStateAuthority);
        SetWaitingTextActive(Object.HasStateAuthority == false);
    }

    private void HideEndScreenUI() => gameObject.SetActive(false);

    private void SetContinueButtonActive(bool isActive) => _continueButton.gameObject.SetActive(isActive);
    private void SetWaitingTextActive(bool isActive) => _waitingText.gameObject.SetActive(isActive);
}