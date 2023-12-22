using UnityEngine;

public class ChangeStateOnSceneLoad : MonoBehaviour {

    [SerializeField] private GameState _stateToSwitchTo;

    private void Start() {
        GameStateManager.ChangeState(_stateToSwitchTo);
    }
}
