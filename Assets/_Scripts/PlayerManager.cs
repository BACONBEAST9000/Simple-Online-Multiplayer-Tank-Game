using Fusion;
using System.Collections.Generic;

public class PlayerManager : NetworkBehaviour {
    
    private static List<Player> _players = new();
    public static List<Player> GetAllPlayers => _players;

    public static void AddPlayer(Player playerToAdd) {
        _players.Add(playerToAdd);
    }
}