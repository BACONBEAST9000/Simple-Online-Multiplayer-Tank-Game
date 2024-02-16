using Fusion;
using UnityEngine;

public static class PlayerColourManager {

    private const string PLAYER_COLOURS_RESOURCES_FILENAME = "PlayerColours";

    private static ColourList _colorManager;

    public static ColourList PlayerColours {
        get {
            if (_colorManager == null) {
                _colorManager = Resources.Load<ColourList>(PLAYER_COLOURS_RESOURCES_FILENAME);
            }
            return _colorManager;
        }
    }


    /// <summary>
    /// Returns colour based on PlayerID which is from 0 to MAX PLAYERS - 1. Host gets first colour in array.
    /// </summary>
    /// <param name="playerRef">Index used to determine colour.</param>
    /// <param name="runner">Used to get max player count.</param>
    /// <returns></returns>
    public static Color GetPlayerColour(PlayerRef playerRef, NetworkRunner runner) {
        // Note that playerRef is current player index with the host being: MAX PLAYERS ALLOWED - 1.
        int colourIndex = (playerRef == runner.SessionInfo.MaxPlayers - 1) ? 0 : playerRef + 1;
        return PlayerColours.Colours[colourIndex];
    }
}