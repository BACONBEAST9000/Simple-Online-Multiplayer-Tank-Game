using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : SimulationBehaviour, INetworkRunnerCallbacks {

    private const string AXIS_HORIZONTAL = "Horizontal";
    private const string AXIS_VERTICAL = "Vertical";
    private const string BUTTON_SHOOT = "Jump";

    private Vector2 _moveInput;

    private bool _toggleReadyInput;

    private void Update() {
        _toggleReadyInput = _toggleReadyInput || Input.GetKeyDown(KeyCode.R);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) {
        PlayerInput playerInputData = new PlayerInput();

        _moveInput.x = Input.GetAxisRaw(AXIS_HORIZONTAL);
        _moveInput.y = Input.GetAxisRaw(AXIS_VERTICAL);

        playerInputData.MoveInput = _moveInput;

        playerInputData.Buttons.Set(ActionButtons.Ready, _toggleReadyInput);
        playerInputData.Buttons.Set(ActionButtons.Shoot, Input.GetButton(BUTTON_SHOOT));

        input.Set(playerInputData);
        
        _toggleReadyInput = false;
    }

    #region Other Unused Callbacks from INetworkRunnerCallbacks
    public void OnConnectedToServer(NetworkRunner runner) {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner) {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {
        
    }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner) {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner) {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {
    }
    #endregion
}
