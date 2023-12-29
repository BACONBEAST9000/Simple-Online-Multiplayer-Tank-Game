using UnityEngine;

public class PlayersInLobbyDisplayManager : PlayerNameAndDetailsDisplayManager {

    protected override void ResubscribeToEvents() {
        base.ResubscribeToEvents();

        Player.OnPlayerToggledReady -= WhenPlayerToggledReady;
        Player.OnPlayerToggledReady += WhenPlayerToggledReady;
    }

    protected override void UnsubscribeFromEvents() {
        base.UnsubscribeFromEvents();

        Player.OnPlayerToggledReady -= WhenPlayerToggledReady;
    }

    private void WhenPlayerToggledReady(Player player, bool isReady) => UpdateReadyState(player, isReady);

    public void UpdateReadyState(Player player, bool isReady) {
        if (!_playerEntries.TryGetValue(player.PlayerID, out PlayerDisplayUI playerDisplay)) {
            Debug.LogWarning("Couldn't find entry to update name of", this);
            return;
        }

        playerDisplay.UpdateEntry(player);
    }
}
