using UnityEngine;

public class SetActiveOnGameState : MonoBehaviour {

    [SerializeField]
    [Tooltip("Object to set active/inactive. If none, defaults to this object.")]
    private GameObject _overrideTargetObject;

    [SerializeField] private GameState _gameStateToCheck;
    
    [Header("If Game State is 'Game State To Check'")]
    [SerializeField] private bool _changeActiveIfGameState;
    [SerializeField] private bool _isActiveWhenGameState;
    
    [Header("Else, If Game State IS NOT 'Game State To Check'")]
    [SerializeField] private bool _changeActiveIfNotGameState;
    [SerializeField] private bool _isActiveWhenNotGameState;

    private void OnEnable() {
        GameStateManager.OnStateChanged -= WhenStateChanges;
        GameStateManager.OnStateChanged += WhenStateChanges;
    }

    private void OnDisable() {
        GameStateManager.OnStateChanged -= WhenStateChanges;
    }

    private void WhenStateChanges(GameState newState) {
        GameObject objectToSetActiveState = _overrideTargetObject ?? gameObject;
        
        if (_changeActiveIfGameState && newState == _gameStateToCheck) {
            objectToSetActiveState.SetActive(_isActiveWhenGameState);
        }
        else if (_changeActiveIfNotGameState) {
            objectToSetActiveState.SetActive(_isActiveWhenNotGameState);
        }
    }
}
