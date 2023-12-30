using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerNameDisplayUI : PlayerDisplayUI {

    [SerializeField] protected TMP_Text PlayerNameText;
    [SerializeField] private Image _backgroundPanel;

    public override void Initalize(Player player) {
        base.Initalize(player);

        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        Player.OnNameUpdated += WhenPlayerNameUpdated;

        if (_backgroundPanel) {
            UIPlayer.GetPlayerVisuals.OnColourChanged -= WhenPlayerColourChanged;
            UIPlayer.GetPlayerVisuals.OnColourChanged += WhenPlayerColourChanged;
            
        }
    }

    private void WhenPlayerColourChanged(Color newColor) {
        _backgroundPanel.color = newColor;
    }

    protected virtual void OnDisable() {
        Player.OnNameUpdated -= WhenPlayerNameUpdated;
        UIPlayer.GetPlayerVisuals.OnColourChanged -= WhenPlayerColourChanged;
    }

    protected void WhenPlayerNameUpdated(Player player, string playerName) => UpdateEntryIfPlayer(player);
}