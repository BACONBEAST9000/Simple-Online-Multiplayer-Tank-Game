using TMPro;
using UnityEngine;

public class PlayerScoreDisplayUI : MonoBehaviour {

    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _scoreText;

    public void UpdateEntry(Player player) {
        var playerData = player.Data;

        UpdateNameText(playerData.PlayerName);
        UpdateScoreText(playerData.Points);
    }

    public void UpdateNameText(string newName) => _playerNameText.text = newName;
    public void UpdateScoreText(int score) => _scoreText.text = score.ToString();
}
