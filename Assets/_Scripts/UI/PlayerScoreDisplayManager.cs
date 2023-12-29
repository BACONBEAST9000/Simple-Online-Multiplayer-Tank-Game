using UnityEngine;

public class PlayerScoreDisplayManager : PlayerNameAndDetailsDisplayManager {
    protected override void ResubscribeToEvents() {
        base.ResubscribeToEvents();

        Player.OnScoreUpdated -= WhenPlayerScoreUpdated;
        Player.OnScoreUpdated += WhenPlayerScoreUpdated;
    }

    protected override void UnsubscribeFromEvents() {
        base.UnsubscribeFromEvents();

        Player.OnScoreUpdated -= WhenPlayerScoreUpdated;
    }

    private void WhenPlayerScoreUpdated(Player player, int newScore) => UpdateScore(player, newScore);
    
    public void UpdateScore(Player player, int newScore) {
        if (!_playerEntries.TryGetValue(player.PlayerID, out PlayerDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to update name of", this);
            return;
        }

        playerDisplay.UpdateEntry(player);
    }
}
