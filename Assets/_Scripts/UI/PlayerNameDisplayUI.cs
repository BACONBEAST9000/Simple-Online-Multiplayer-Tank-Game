using TMPro;
using UnityEngine;

public abstract class PlayerNameDisplayUI : PlayerDisplayUI {

    [SerializeField] protected TMP_Text PlayerNameText;

    public override void Initalize(Player player) {
        base.Initalize(player);

        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnNameUpdated += WhenPlayerNameUpdated;
    }

    protected virtual void OnDisable() {
        Player.OnNameUpdated -= WhenPlayerNameUpdated;
    }

    protected void WhenPlayerNameUpdated(Player player, string playerName) => UpdateEntryIfPlayer(player);
}