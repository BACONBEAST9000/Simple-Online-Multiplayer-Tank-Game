using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private Button _hostGameButton;
    [SerializeField] private Button _joinGameButton;

    private void Awake() {
        _hostGameButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.StartHostSession();
        });
        
        _joinGameButton.onClick.AddListener(() => {
            MultiplayerSessionManager.Instance.StartClientSession();
        });
    }

    private void OnDestroy() {
        _hostGameButton.onClick.RemoveAllListeners();
        _joinGameButton.onClick.RemoveAllListeners();
    }
}
