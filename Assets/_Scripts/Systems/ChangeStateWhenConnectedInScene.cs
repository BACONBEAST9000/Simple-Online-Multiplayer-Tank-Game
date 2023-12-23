using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeStateWhenConnectedInScene : NetworkBehaviour {

    [SerializeField] private Scenes _sceneWhereStateChanges;
    [SerializeField] private GameState _gameStateToChangeTo;

    public override void Spawned() {
        UpdateStateIfScene();
    }

    private void UpdateStateIfScene() {
        if (CurrentSceneMatches()) {
            GameStateManager.ChangeState(_gameStateToChangeTo);
        }
    }

    private bool CurrentSceneMatches() => SceneManager.GetSceneAt((int)_sceneWhereStateChanges) == SceneManager.GetActiveScene();
}

public enum Scenes {
    MainMenu,
    GameScene,
}