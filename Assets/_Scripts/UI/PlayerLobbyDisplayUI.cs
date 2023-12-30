using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyDisplayUI : PlayerNameDisplayUI {

    [SerializeField] private TMP_Text _readyText;
    [SerializeField] private Button _kickButton;

    public override void Initalize(Player player) {
        base.Initalize(player);
        
        Player.OnPlayerToggledReady -= WhenPlayerTogglesReady;
        Player.OnPlayerToggledReady += WhenPlayerTogglesReady;

        _kickButton.gameObject.SetActive(!player.IsHost && player.HasStateAuthority);

        _kickButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.KickPlayer(player);
        });
    }

    protected override void OnDisable() {
        base.OnDisable();

        Player.OnPlayerToggledReady -= WhenPlayerTogglesReady;

        _kickButton.onClick?.RemoveAllListeners();
    }

    private void WhenPlayerTogglesReady(Player playerWhoToggledReady, bool isReady) => UpdateEntryIfPlayer(playerWhoToggledReady);

    public override void UpdateEntry(Player player) {
        UpdateNameText(player.NickName.ToString());
        UpdateReadyText(player.IsReady);
    }

    private void UpdateNameText(string newName) => PlayerNameText.text = newName;
    private void UpdateReadyText(bool isReady) => _readyText.text = (isReady) ? "READY!" : "Not ready...";
}
