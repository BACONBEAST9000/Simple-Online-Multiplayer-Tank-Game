using Fusion;
using TMPro;
using UnityEngine;

public class RoomNameLabel : NetworkBehaviour {

    private const int MAX_CHARACTERS_UNTIL_TOO_MUCH = 15;

    [SerializeField] private GameObject _roomNameUI;
    [SerializeField] private TMP_Text _label;

    public override void Spawned() {
        string roomName = Runner.SessionInfo.Name;
        bool roomNameTooLong = roomName.Length > MAX_CHARACTERS_UNTIL_TOO_MUCH;

        if (roomNameTooLong) {
            _roomNameUI.SetActive(false);
            _label.gameObject.SetActive(false);
            return;
        }

        _label.text = roomName;
    }
}
