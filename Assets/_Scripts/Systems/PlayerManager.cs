using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Universally used class that has a reference to each player! Called from Spawned() and Despawned() in the Player class.
/// </summary>
public static class PlayerManager {

    //public static PlayerManager Instance;

    public static Dictionary<PlayerRef, Player> _players = new();
    public static Dictionary<PlayerRef, Player> GetAllPlayers => _players;
    public static List<PlayerRef> GetAllPlayersReferencesStartingWithHost() {
        List<PlayerRef> playerRefs = GetAllPlayers.Keys.ToList();

        playerRefs.Insert(0, playerRefs[playerRefs.Count - 1]);
        playerRefs.RemoveAt(playerRefs.Count - 1);

        return playerRefs;
    }

    public static int GetPlayerCount => GetAllPlayers.Count;

    //private void Awake() {
    //    if (Instance != null) {
    //        Destroy(Instance);
    //    }

    //    Instance = this;
    //}

    public static void AddPlayer(Player playerToAdd) {
        if (_players.ContainsKey(playerToAdd.PlayerID)) {
            Debug.LogWarning($"[Add] Player ({playerToAdd.NickName}, {playerToAdd.PlayerID}) already added! Returning.");
            return;
        }
        
        _players.Add(playerToAdd.PlayerID, playerToAdd);
    }

    public static void RemovePlayer(Player playerToRemove) {
        if (!_players.ContainsKey(playerToRemove.PlayerID)) {
            Debug.LogWarning($"[Remove] Player ({playerToRemove.NickName}, {playerToRemove.PlayerID}) not found! Returning.");
            return;
        }

        _players.Remove(playerToRemove.PlayerID);
    }

    public static void UpdatePlayerWithReference(PlayerRef playerRefToUpdate, Player newPlayer) {
        if (!_players.ContainsKey(playerRefToUpdate)) {
            Debug.LogWarning($"[UpdatePlayerWithReference] Key: ({playerRefToUpdate}) not found! Can't update value with Player ({newPlayer.NickName}, {newPlayer.PlayerID}). Returning.");
            return;
        }
        _players[playerRefToUpdate] = newPlayer;
    }

    public static Player GetPlayerWithReference(PlayerRef playerRef) {
        return GetAllPlayers.Where(player => player.Key == playerRef).FirstOrDefault().Value;
    }

    public static List<Player> GetPlayersInOrderOfDescendingScore => GetAllPlayers.Values.OrderByDescending(player => player.Score).ToList();
}