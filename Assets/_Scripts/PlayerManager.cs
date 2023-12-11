using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : NetworkBehaviour {
    
    private static List<Player> _players = new();
    public static List<Player> GetAllPlayers => _players;

    public static void AddPlayer(Player playerToAdd) {
        _players.Add(playerToAdd);
    }

    public override void FixedUpdateNetwork() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            print("-=-= Players -=-=");
            foreach (Player player in _players) {
                print($"Player in Players List: {player.NickName}");
            }
        }
    }
}