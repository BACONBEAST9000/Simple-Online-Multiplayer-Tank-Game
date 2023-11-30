using TMPro;
using UnityEngine;

public class PlayerScoreDisplay : MonoBehaviour {

    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _scoreText;

    public void AddEntry(PlayerData playerData) {
        _playerNameText.text = playerData.PlayerName;
        _scoreText.text = $"Lives: {playerData.Points}";
    }
}
