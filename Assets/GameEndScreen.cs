using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class GameEndScreen : MonoBehaviour {

    [SerializeField] private TMP_Text[] _playerPositionTextElements;
    [SerializeField] private Color[] _positionColours;

    private List<TestPlayer> _testPlayers = new List<TestPlayer>();

    private string[] _ordinalNumbers = { "1st", "2nd", "3rd", "4th" };
    private float[] _fontSizes = { 72, 64, 56, 42 };

    private void Start() {
        TestPlayer player1 = new TestPlayer("Alice", 6);
        TestPlayer player2 = new TestPlayer("Bob", 1);
        TestPlayer player3 = new TestPlayer("Carrie", 7);
        TestPlayer player4 = new TestPlayer("Doug", 6);

        _testPlayers.Add(player1);
        _testPlayers.Add(player2);
        _testPlayers.Add(player3);
        _testPlayers.Add(player4);

        _testPlayers = _testPlayers.OrderByDescending(player => player.Score).ToList();
        DisplayPlayers();
    }

    private void DisplayPlayers() {
        int positionIndex = 0;
        int currentScore = _testPlayers[0].Score;

        for (int i = 0; i < _testPlayers.Count; i++) {
            TestPlayer player = _testPlayers[i];
            if (currentScore != player.Score) {
                currentScore = player.Score;
                positionIndex++;
            }

            _playerPositionTextElements[i].text = $"{_ordinalNumbers[positionIndex]} {player.Name}";
            _playerPositionTextElements[i].fontSize = _fontSizes[positionIndex];
            _playerPositionTextElements[i].color = _positionColours[positionIndex];
        }
    }
}

public struct TestPlayer {
    public string Name;
    public int Score;

    public TestPlayer(string name, int score) {
        Name = name;
        Score = score;
    }
}
