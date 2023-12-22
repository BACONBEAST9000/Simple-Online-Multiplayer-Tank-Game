using Fusion;
using UnityEngine;

public class PlayerColourManager : NetworkBehaviour {

    public static Color[] PlayerColours { get; private set; }

    // Only to be copied into a static array. Used to set in editor.
    [SerializeField] private Color[] _setPlayerColours;

    private void Awake() {
        if (PlayerColours == null) {
            PlayerColours = _setPlayerColours;
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
        return PlayerColours[colourIndex];
    }
}