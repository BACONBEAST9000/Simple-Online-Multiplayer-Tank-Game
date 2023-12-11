using Fusion;
using System.Collections.Generic;

public class ReadyUpManager : NetworkBehaviour {

    private static List<Player> _readyPlayers = new();

    private void OnEnable() {
        Player.OnPlayerToggledReady -= WhenPlayerTogglesReady;
        Player.OnPlayerToggledReady += WhenPlayerTogglesReady;
    }

    private void OnDisable() {
        Player.OnPlayerToggledReady -= WhenPlayerTogglesReady;
    }

    private void WhenPlayerTogglesReady(Player player, bool isPlayerReady) {
        if (isPlayerReady) {
            ReadyPlayer(player);
        }
        else {
            UnreadyPlayer(player);
        }
    }

    private bool _displayedReadyToPlay = false;
    public override void FixedUpdateNetwork() {
        if (!_displayedReadyToPlay && _readyPlayers.Count == PlayerManager.GetAllPlayers.Count) {
            print("ALL PLAYERS ARE READY TO PLAY!");
            _displayedReadyToPlay = true;
        }
    }

    public void ReadyPlayer(Player readyPlayer) {
        _readyPlayers.Add(readyPlayer);
    }

    public void UnreadyPlayer(Player notReadyPlayer) {
        _readyPlayers.Remove(notReadyPlayer);
    }
}
