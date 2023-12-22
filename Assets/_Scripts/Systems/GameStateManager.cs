using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    private static GameStateManager PrivateInstance;

    public static event Action<GameState> OnStateChanged;
    public static GameState CurrentState { get; private set; }

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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            print("Current State: " + CurrentState);
        }
    }

    public static void ChangeState(GameState newState) {
        CurrentState = newState;
        print("GAME STATE CHANGED: " + newState);
        OnStateChanged?.Invoke(newState);
    }
}
