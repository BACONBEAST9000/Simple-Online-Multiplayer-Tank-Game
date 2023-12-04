using TMPro;
using UnityEngine;

public class PlayerScoreDisplayUI : MonoBehaviour {

    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _scoreText;

    public void UpdateEntry(Player player) {
        UpdateNameText(player.NickName.ToString());
        UpdateScoreText(player.Score);
    }

    public void UpdateNameText(string newName) => _playerNameText.text = newName;
    public void UpdateScoreText(int score) => _scoreText.text = score.ToString();
}
