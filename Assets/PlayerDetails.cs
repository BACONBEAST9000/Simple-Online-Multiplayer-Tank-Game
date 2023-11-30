using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour {

    [SerializeField] private RectTransform _parentUIObject;
    [SerializeField] private PlayerScoreDisplay _scoreDisplayPrefab;

    private List<PlayerScoreDisplay> _elements = new();

    private void OnEnable() {
        MultiplayerSessionManager.OnPlayerJoinedGame -= WhenPlayerJoinsGame;
        MultiplayerSessionManager.OnPlayerJoinedGame += WhenPlayerJoinsGame;

        MultiplayerSessionManager.OnPlayerConnectedToGame -= WhenPlayerJoinsGame;
        MultiplayerSessionManager.OnPlayerConnectedToGame += WhenPlayerJoinsGame;
    }

    private void OnDisable() {
        MultiplayerSessionManager.OnPlayerJoinedGame -= WhenPlayerJoinsGame;
        MultiplayerSessionManager.OnPlayerConnectedToGame -= WhenPlayerJoinsGame;
    }
    
    private void WhenPlayerJoinsGame() {
        foreach (var element in _elements) {
            Destroy(element.gameObject);
        }
        _elements.Clear();

        var allPlayers = MultiplayerSessionManager.Instance.SpawnedPlayers;

        PlayerData currentPlayerData;
        foreach (var entry in allPlayers) {
            currentPlayerData = entry.Value.Data;
            print($"{currentPlayerData.PlayerName} has {currentPlayerData.Points} lives.");

            PlayerScoreDisplay newDisplay = Instantiate(_scoreDisplayPrefab, _parentUIObject);
            newDisplay.AddEntry(currentPlayerData);
            _elements.Add(newDisplay);
        }

    }
}
