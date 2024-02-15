using UnityEngine;

public class LocalPlayerData : MonoBehaviour {
    private string _nickname;

    public string NickName {
        get {
            if (string.IsNullOrWhiteSpace(_nickname)) {
                _nickname = GetRandomNickName();
            }
            
            return _nickname;
        }

        set => _nickname = value;
    }

    public static string GetRandomNickName() {
        return "Player " + Random.Range(0, 10000);
    }

    private void Awake() {
        int instancesCount = FindObjectsOfType<LocalPlayerData>().Length;

        if (instancesCount > 1) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}