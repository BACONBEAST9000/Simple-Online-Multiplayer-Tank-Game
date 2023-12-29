using TMPro;
using UnityEngine;

public class PlayerScoreDisplayUI : PlayerDisplayUI {

    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _scoreText;

    public override void Initalize(Player player) {
        base.Initalize(player);

        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnNameUpdated += WhenPlayerNameUpdated;

        Player.OnScoreUpdated -= WhenPlayerScoreUpdated;
        Player.OnScoreUpdated += WhenPlayerScoreUpdated;
    }

    private void OnDisable() {
        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnScoreUpdated -= WhenPlayerScoreUpdated;
    }


    private void WhenPlayerNameUpdated(Player playerWithUpdatedName, string playerName) => UpdateEntryIfPlayer(playerWithUpdatedName);

    private void WhenPlayerScoreUpdated(Player playerWithUpdatedScore, int newScore) => UpdateEntryIfPlayer(playerWithUpdatedScore);
    
    public override void UpdateEntry(Player player) {
        UpdateNameText(player.NickName.ToString());
        UpdateScoreText(player.Score);
    }

    public void UpdateNameText(string newName) => _playerNameText.text = newName;
    public void UpdateScoreText(int score) => _scoreText.text = score.ToString();
}
