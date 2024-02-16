using UnityEngine;

public class Billboard : MonoBehaviour {
    private Camera _camera;
    
    private void Awake() {
        _camera = Camera.main;
    }

    private void Update() {
        if (_camera) {
            transform.forward = _camera.transform.forward;
        }
    }
}
