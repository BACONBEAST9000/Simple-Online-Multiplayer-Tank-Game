using System;

public static class GameStateManager {

    public static event Action<GameState> OnStateChanged;
    public static GameState CurrentState { get; private set; } = GameState.Menu;
    public static GameState PreviousState { get; private set; }

    public static void ChangeState(GameState newState) {
        if (newState == CurrentState) {
            return;
        }

        PreviousState = CurrentState;
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }
}
