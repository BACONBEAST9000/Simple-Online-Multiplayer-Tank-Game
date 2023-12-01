using System.Text;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _debugText;

    private StringBuilder _logStringBuilder = new StringBuilder();

    private void OnEnable() {
        SetEvents();
    }

    private void OnDisable() {
        ClearEvents();
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
}
