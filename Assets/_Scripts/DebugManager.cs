using System.Text;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour {
    private static DebugManager Instance;

    [SerializeField] private TextMeshProUGUI _debugText;

    private StringBuilder _logStringBuilder = new StringBuilder();

    private void OnEnable() {
        SetEvents();
    }

    private void OnDisable() {
        ClearEvents();
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void SetEvents() {
        Application.logMessageReceived += WhenLogMessageReceived;
    }

    private void ClearEvents() {
        Application.logMessageReceived -= WhenLogMessageReceived;
    }
    
    private void WhenLogMessageReceived(string condition, string stackTrace, LogType type) {
        _logStringBuilder.Append(condition + "\n");
        _debugText.text = _logStringBuilder.ToString();
    }

    public void Clear() {
        _logStringBuilder.Clear();
        _debugText.text = string.Empty;
    }
}
