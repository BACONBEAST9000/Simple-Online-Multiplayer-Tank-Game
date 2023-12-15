using UnityEngine;
using Fusion;

public class BulletVisual : NetworkBehaviour {

    // Property name may change based on material shader used. Assuming default is used.
    private const string EMISSION_COLOR_PROPERTY_NAME = "_EmissionColor";

    [SerializeField] private MeshRenderer _renderer;

    private void OnEnable() {
        PlayerShoot.OnPlayerShotBullet -= WhenPlayerShoots;
        PlayerShoot.OnPlayerShotBullet += WhenPlayerShoots;
    }

    private void OnDisable() {
        PlayerShoot.OnPlayerShotBullet -= WhenPlayerShoots;
    }

    private void WhenPlayerShoots(Bullet bullet, Player player) {
        RPC_SetMaterialColour(player.GetPlayerVisuals.PlayerColour);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetMaterialColour(Color colour) {
        _renderer.material.color = colour;
        _renderer.material.SetColor(EMISSION_COLOR_PROPERTY_NAME, colour);
    }
}
