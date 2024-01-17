using UnityEngine;
using Fusion;

public class BulletVisual : NetworkBehaviour {

    [SerializeField] private MeshRenderer _renderer;

    private void OnEnable() {
        PlayerShoot.OnPlayerShotBullet -= WhenPlayerShoots;
        PlayerShoot.OnPlayerShotBullet += WhenPlayerShoots;
    }

    private void OnDisable() {
        PlayerShoot.OnPlayerShotBullet -= WhenPlayerShoots;
    }

    private void WhenPlayerShoots(Bullet bullet, Player player) {
        RPC_SetMaterialColour(player.Visuals.PlayerColour);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetMaterialColour(Color colour) {
        _renderer.material.color = colour;
        _renderer.material.SetColor(Utils.EMISSION_COLOR_PROPERTY_NAME, colour);
    }
}