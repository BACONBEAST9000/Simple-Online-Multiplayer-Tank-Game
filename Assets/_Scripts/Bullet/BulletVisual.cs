using UnityEngine;
using Fusion;

public class BulletVisual : NetworkBehaviour {

    [SerializeField] private Bullet _thisBullet;
    [SerializeField] private MeshRenderer _renderer;

    private void OnEnable() {
        PlayerShoot.OnPlayerShotBullet -= WhenPlayerShoots;
        PlayerShoot.OnPlayerShotBullet += WhenPlayerShoots;
    }

    private void OnDisable() {
        PlayerShoot.OnPlayerShotBullet -= WhenPlayerShoots;
    }

    private void WhenPlayerShoots(Bullet bulletShot, Player playerThatShot) {
        if (bulletShot != _thisBullet) {
            return;
        }

        RPC_SetMaterialColour(playerThatShot.Visuals.PlayerColour);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetMaterialColour(Color colour) {
        _renderer.material.color = colour;
        _renderer.material.SetColor(Utils.EMISSION_COLOR_PROPERTY_NAME, colour);
    }
}