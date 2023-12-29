using UnityEngine;

public abstract class PlayerDisplayUI : MonoBehaviour {

    protected Player UIPlayer;

    public virtual void Initalize(Player player) {
        UIPlayer = player;
        UpdateEntry(player);
    }

    protected virtual void UpdateEntryIfPlayer(Player player) {
        if (player != UIPlayer) {
            return;
        }

        UpdateEntry(player);
    }

    public abstract void UpdateEntry(Player player);
}