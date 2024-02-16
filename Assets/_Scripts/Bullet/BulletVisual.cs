using UnityEngine;
using Fusion;

public class BulletVisual : NetworkBehaviour {

    [SerializeField] private Bullet _thisBullet;
    [SerializeField] private MeshRenderer _renderer;

    [Networked(OnChanged = nameof(OnBulletColourChanged))]
    public Color BulletColour { get; private set; }

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

        BulletColour = playerThatShot.Visuals.PlayerColour;
    }

    public static void OnBulletColourChanged(Changed<BulletVisual> changed) {
        Color newColour = changed.Behaviour.BulletColour;

        changed.LoadOld();
        Color oldColour = changed.Behaviour.BulletColour;

        if (newColour == oldColour) return;

        BulletVisual bulletVisual = changed.Behaviour;
        bulletVisual.RPC_SetMaterialColour(newColour);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetMaterialColour(Color colour) {
        _renderer.material.color = colour;
        _renderer.material.SetColor(Utils.EMISSION_COLOR_PROPERTY_NAME, colour);
    }
}