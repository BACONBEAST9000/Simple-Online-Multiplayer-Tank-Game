using UnityEngine;

public class PlayerNameAndDetailsDisplayManager : PlayerDetailsDisplayManager {
    protected override void ResubscribeToEvents() {
        base.ResubscribeToEvents();

        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnNameUpdated += WhenPlayerNameUpdated;
    }

    protected override void UnsubscribeFromEvents() {
        base.UnsubscribeFromEvents();

        Player.OnNameUpdated -= WhenPlayerNameUpdated;
    }

    private void WhenPlayerNameUpdated(Player player, string newName) => UpdateName(player, newName);

    protected virtual void UpdateName(Player player, string newName) {
        if (!_playerEntries.TryGetValue(player.PlayerID, out PlayerDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to update name of", this);
            return;
        }

        playerDisplay.UpdateEntry(player);
    }
}