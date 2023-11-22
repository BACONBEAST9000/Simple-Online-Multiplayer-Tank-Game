using System;
using UnityEngine;

public class HandlePlayerInput : MonoBehaviour {

    public event Action<PlayerInput> OnPlayerInput;

    private Vector2 _moveInput;
    private bool _shootInput;

    private void Update() {
        //CheckInput();
    }


    private void CheckInput() {
        PlayerInput input = new PlayerInput();
      
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        _shootInput = Input.GetKeyDown(KeyCode.Space);

        input.MoveInput = _moveInput;

        if (_shootInput) {
            input.Buttons |= PlayerInput.SHOOT_INPUT;
        }
        
        OnPlayerInput?.Invoke(input);
    }
}