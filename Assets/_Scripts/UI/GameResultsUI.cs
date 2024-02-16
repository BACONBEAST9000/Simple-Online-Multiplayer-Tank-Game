using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class GameResultsUI : NetworkBehaviour {
    [Header("List of Players")]
    [SerializeField] private ResultsEntryTextUI[] _orderedPlayerEntries;

    private void OnEnable() {
        HideAllTextEntries();
        DisplayResults();
    }

    private void HideAllTextEntries() {
        foreach (var textElement in _orderedPlayerEntries) {
            textElement.gameObject.SetActive(false);
        }
    }

    private void DisplayResults() {
        List<Player> playersOrderedByScore = PlayerManager.GetPlayersInOrderOfDescendingScore;

        int positionIndex = 0;
        int currentScore = playersOrderedByScore[0].Scoring.Score;

        for (int i = 0; i < playersOrderedByScore.Count; i++) {
            Player player = playersOrderedByScore[i];

            bool twoPlayerScoresAreNotTied = currentScore != player.Scoring.Score;
            if (twoPlayerScoresAreNotTied) {
                currentScore = player.Scoring.Score;
                positionIndex++;
            }

            _orderedPlayerEntries[i].SetAndDisplayEntry(player, positionIndex);
        }
    }
}
