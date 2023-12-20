using TMPro;
using UnityEngine;

public class PlayerScoreDisplayUI : PlayerDisplayUI {

    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _scoreText;

    public override void UpdateEntry(Player player) {
        UpdateNameText(player.NickName.ToString());
        UpdateScoreText(player.Score);
    }

    public void UpdateNameText(string newName) => _playerNameText.text = newName;
    public void UpdateScoreText(int score) => _scoreText.text = score.ToString();
}
