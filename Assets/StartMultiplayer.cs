using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMultiplayer : MonoBehaviour {

    private const string SESSION_NAME = "TestRoom";

    private NetworkRunner _runner;

    private async void StartGame(GameMode mode) {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        StartGameArgs game = new StartGameArgs() {
            GameMode = mode,
            SessionName = SESSION_NAME,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        };
        await _runner.StartGame(game);
        print("Game Started!");
    }

    private void OnGUI() {
        HostAndClientButtons();
    }

    private void HostAndClientButtons() {
        if (_runner != null) {
            return;
        }

        if (GUI.Button(new Rect(0, 0, 200, 40), "Host")) {
            StartGame(GameMode.Host);
        }

        if (GUI.Button(new Rect(0, 50, 200, 40), "Client")) {
            StartGame(GameMode.Client);
        }
    }
}
