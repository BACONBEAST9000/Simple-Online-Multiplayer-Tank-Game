using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    private static GameStateManager PrivateInstance;

    public static event Action<GameState> OnStateChanged;
    public static GameState CurrentState { get; private set; }
    public static GameState PreviousState { get; private set; }

    private void Awake() {
        if (PrivateInstance == null) {
            PrivateInstance = this;
            DontDestroyOnLoad(gameObject);
            ChangeState(GameState.Menu);
        }
        else {
            Destroy(this);
        }
    }

    public static void ChangeState(GameState newState) {
        if (newState == CurrentState) {
            return;
        }

        PreviousState = CurrentState;
        CurrentState = newState;

        print("GAME STATE CHANGED: " + newState);
        OnStateChanged?.Invoke(newState);
    }
}
