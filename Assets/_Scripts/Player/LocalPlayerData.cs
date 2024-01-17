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
        return "Player " + UnityEngine.Random.Range(0, 10000);
    }

    // TODO: Turn this into a proper singleton?
    private void Awake() {
        int instancesCount = FindObjectsOfType<LocalPlayerData>().Length;

        if (instancesCount > 1) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            print(NickName);
        }
    }
}