using TMPro;
using UnityEngine;

public class PlayerScoreDisplayUI : PlayerNameDisplayUI {

    [SerializeField] private TMP_Text _scoreText;

    public override void Initalize(Player player) {
        base.Initalize(player);

        PlayerScore.OnScoreUpdated -= WhenPlayerScoreUpdated;
        PlayerScore.OnScoreUpdated += WhenPlayerScoreUpdated;
    }

    protected override void OnDisable() {
        base.OnDisable();

        PlayerScore.OnScoreUpdated -= WhenPlayerScoreUpdated;
    }

    private void WhenPlayerScoreUpdated(Player playerWithUpdatedScore, int newScore) => UpdateEntryIfPlayer(playerWithUpdatedScore);
    
    public override void UpdateEntry(Player player) {
        UpdateNameText(player.NickName.ToString());
        UpdateScoreText(player.Scoring.Score);
    }

    public void UpdateNameText(string newName) => PlayerNameText.text = newName;
    public void UpdateScoreText(int score) => _scoreText.text = score.ToString();
}
