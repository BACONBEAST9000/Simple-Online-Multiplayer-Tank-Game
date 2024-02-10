using Fusion;

public class SyncHostGameStateWithClients : NetworkBehaviour {

    [Networked(OnChanged = nameof(OnHostStateChanged))]
    public GameState HostGameState { get; private set; } = GameState.DEFAULT;

    private void OnEnable() {
        GameStateManager.OnStateChanged -= WhenStateChanges;
        GameStateManager.OnStateChanged += WhenStateChanges;
    }

    private void OnDisable() {
        GameStateManager.OnStateChanged -= WhenStateChanges;
    }

    private static void OnHostStateChanged(Changed<SyncHostGameStateWithClients> changed) {
        GameState currentState = changed.Behaviour.HostGameState;

        changed.LoadOld();
        GameState previousState = changed.Behaviour.HostGameState;

        if (currentState != previousState) {
            GameStateManager.ChangeState(currentState);
        }
    }

    private void WhenStateChanges(GameState newState) {
        if (!HasStateAuthority) return;
        
        HostGameState = newState;
    }
}
