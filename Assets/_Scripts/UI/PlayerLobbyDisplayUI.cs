using TMPro;
using UnityEngine;

public class PlayerLobbyDisplayUI : PlayerDisplayUI {

    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _readyText;

    public override void Initalize(Player player) {
        base.Initalize(player);

        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnNameUpdated += WhenPlayerNameUpdated;

        Player.OnPlayerToggledReady -= WhenPlayerTogglesReady;
        Player.OnPlayerToggledReady += WhenPlayerTogglesReady;
    }

    private void OnDisable() {
        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnPlayerToggledReady -= WhenPlayerTogglesReady;
    }

    private void WhenPlayerNameUpdated(Player playerWithUpdatedName, string playerName) {
        if (playerWithUpdatedName != UIPlayer) {
            return;
        }

        UpdateEntry(playerWithUpdatedName);
    }

    private void WhenPlayerTogglesReady(Player playerWhoToggledReady, bool isReady) {
        if (playerWhoToggledReady != UIPlayer) {
            return;
        }
        
        UpdateEntry(playerWhoToggledReady);
    }

    public override void UpdateEntry(Player player) {
        UpdateNameText(player.NickName.ToString());
        UpdateReadyText(player.IsReady);
    }

    private void UpdateNameText(string newName) => _playerNameText.text = newName;
    private void UpdateReadyText(bool isReady) => _readyText.text = (isReady) ? "READY!" : "Not ready...";
}
