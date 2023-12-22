using UnityEngine;

public class SetActiveOnGameState : MonoBehaviour {

    [SerializeField] private GameState _gameStateToCheck;
    
    [Header("If current game state is 'game state to check'")]
    [SerializeField] private bool _changeActiveIfGameState;
    [SerializeField] private bool _isActiveWhenGameState;
    
    [Header("If current game state ISN'T 'game state to check'")]
    [SerializeField] private bool _changeActiveIfNotGameState;
    [SerializeField] private bool _isActiveWhenNotGameState;

    private void Awake() {
        if (_changeActiveIfGameState && GameStateManager.CurrentState == _gameStateToCheck) {
            gameObject.SetActive(_isActiveWhenGameState);
        }
        else if (_changeActiveIfNotGameState) {
            gameObject.SetActive(_isActiveWhenNotGameState);
        }
    }
}
