using TMPro;
using UnityEngine;

public class PlayerLobbyDisplayUI : PlayerNameDisplayUI {

    [SerializeField] private TMP_Text _readyText;

    public override void Initalize(Player player) {
        base.Initalize(player);
        
        Player.OnPlayerToggledReady -= WhenPlayerTogglesReady;
        Player.OnPlayerToggledReady += WhenPlayerTogglesReady;
    }

    protected override void OnDisable() {
        base.OnDisable();

        Player.OnPlayerToggledReady -= WhenPlayerTogglesReady;
    }

    private void WhenPlayerTogglesReady(Player playerWhoToggledReady, bool isReady) => UpdateEntryIfPlayer(playerWhoToggledReady);

    public override void UpdateEntry(Player player) {
        UpdateNameText(player.NickName.ToString());
        UpdateReadyText(player.IsReady);
    }

    private void UpdateNameText(string newName) => PlayerNameText.text = newName;
    private void UpdateReadyText(bool isReady) => _readyText.text = (isReady) ? "READY!" : "Not ready...";
}
