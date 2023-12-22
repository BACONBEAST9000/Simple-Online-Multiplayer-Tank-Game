using TMPro;
using UnityEngine;

public class PlayerLobbyDisplayUI : PlayerDisplayUI {

    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _readyText;

    public override void UpdateEntry(Player player) {
        UpdateNameText(player.NickName.ToString());
        UpdateReadyText(player.IsReady);
    }

    public void UpdateNameText(string newName) => _playerNameText.text = newName;
    public void UpdateReadyText(bool isReady) => _readyText.text = (isReady) ? "READY!" : "Not ready...";
}
