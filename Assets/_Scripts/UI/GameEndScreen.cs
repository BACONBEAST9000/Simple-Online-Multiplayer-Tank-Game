using Fusion;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScreen : NetworkBehaviour {

    private static string[] _ordinalNumbers = { "1st", "2nd", "3rd", "4th" };
    private static float[] _fontSizes = { 72, 64, 56, 42 };

    [SerializeField] private RectTransform _waitingText;
    [SerializeField] private RectTransform _resultsUI;
    [SerializeField] private RectTransform _allPlayersLeftUI;

    [Header("Buttons")]
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _quitButton;

    [Header("List of Players")]
    [SerializeField] private TMP_Text[] _playerPositionTextElements;
    [SerializeField] private Color[] _positionColours;

    private void OnEnable() {
        if (PlayerManager.IsEnoughPlayersToStartGame) {
            _resultsUI.gameObject.SetActive(true);
            _allPlayersLeftUI.gameObject.SetActive(false);

            HideAllTextEntries();
            DisplayResults();
        }
        else {
            _allPlayersLeftUI.gameObject.SetActive(true);
            _resultsUI.gameObject.SetActive(false);
        }

        ShowContinueButtonOrWaitingText();

        _continueButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.LoadMenuScene();
        });

        _quitButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.ShutdownSession();
        });
    }

    private void OnDisable() {
        _continueButton?.onClick.RemoveAllListeners();
        _quitButton?.onClick.RemoveAllListeners();
    }

    private void HideAllTextEntries() {
        foreach (var textElement in _playerPositionTextElements) {
            textElement.gameObject.SetActive(false);
        }
    }

    private void DisplayResults() {
        List<Player> playersOrderedByScore = PlayerManager.GetPlayersInOrderOfDescendingScore;
        
        int positionIndex = 0;
        int currentScore = playersOrderedByScore[0].Score;

        for (int i = 0; i < playersOrderedByScore.Count; i++) {
            Player player = playersOrderedByScore[i];
            if (currentScore != player.Score) {
                currentScore = player.Score;
                positionIndex++;
            }

            // TODO: Refactor
            _playerPositionTextElements[i].text = $"{_ordinalNumbers[positionIndex]} {player.NickName}";
            _playerPositionTextElements[i].fontSize = _fontSizes[positionIndex];
            _playerPositionTextElements[i].color = _positionColours[positionIndex];
            _playerPositionTextElements[i].gameObject.SetActive(true);
        }
    }
    
    private void ShowContinueButtonOrWaitingText() {
        SetContinueButtonActive(Object.HasStateAuthority);
        SetWaitingTextActive(Object.HasStateAuthority == false);
    }

    private void SetContinueButtonActive(bool isActive) => _continueButton.gameObject.SetActive(isActive);
    private void SetWaitingTextActive(bool isActive) => _waitingText.gameObject.SetActive(isActive);
}