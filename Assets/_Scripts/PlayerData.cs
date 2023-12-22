using UnityEngine;

public class PlayerData : MonoBehaviour {
    private string _nickname;

    public string NickName {
        get {
            return (string.IsNullOrWhiteSpace(_nickname))
                ? GetRandomNickName()
                : _nickname;
        }

        set { _nickname = value; }
    }

    public static string GetRandomNickName() {
        return "Player " + UnityEngine.Random.Range(0, 10000);
    }

    private void Awake() {
        int instancesCount = FindObjectsOfType<PlayerData>().Length;

        if (instancesCount > 1) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}