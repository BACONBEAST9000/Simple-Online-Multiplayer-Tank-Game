using UnityEngine;

public abstract class PlayerDisplayUI : MonoBehaviour {

    protected Player UIPlayer;

    public virtual void Initalize(Player player) {
        UIPlayer = player;
        UpdateEntry(player);
    }

    public abstract void UpdateEntry(Player player);
}