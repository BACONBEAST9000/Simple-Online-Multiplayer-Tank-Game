using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndScreen : MonoBehaviour {

    private static string[] _ordinalNumbers = { "1st", "2nd", "3rd", "4th" };
    private static float[] _fontSizes = { 72, 64, 56, 42 };
    
    [SerializeField] private TMP_Text[] _playerPositionTextElements;
    [SerializeField] private Color[] _positionColours;

    private void OnEnable() {
        HideAllTextEntries();
        DisplayResults();
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

            _playerPositionTextElements[i].text = $"{_ordinalNumbers[positionIndex]} {player.NickName}";
            _playerPositionTextElements[i].fontSize = _fontSizes[positionIndex];
            _playerPositionTextElements[i].color = _positionColours[positionIndex];
            _playerPositionTextElements[i].gameObject.SetActive(true);
        }
    }
}