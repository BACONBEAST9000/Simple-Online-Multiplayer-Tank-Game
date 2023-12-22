using Fusion;
using System;
using UnityEngine;

public class PlayerVisuals : NetworkBehaviour {
    
    // Property name may change based on material shader used. Assuming default is used.
    private const string EMISSION_COLOR_PROPERTY_NAME = "_EmissionColor";
    
    [SerializeField] private MeshRenderer[] _meshRenderers;
    [SerializeField] private ParticleSystem _destroyedSFX;

    public ParticleSystem GetDestroyedParticleEffect => _destroyedSFX;

    [Networked(OnChanged = nameof(OnPlayerColourChanged))]
    public Color PlayerColour { get; private set; }

    public override void Spawned() {
        PlayerColour = PlayerColourManager.GetPlayerColour(Object.InputAuthority, Runner);
    }

    public void DestroyedEffect() {
        HidePlayer();
        _destroyedSFX.Play();
    }

    public void ShowPlayer() => TogglePlayerVisibility(true);
    public void HidePlayer() => TogglePlayerVisibility(false);
    public void TogglePlayerVisibility(bool isVisible) => ForEachMeshRenderer((MeshRenderer renderer) => renderer.enabled = isVisible);

    public static void OnPlayerColourChanged(Changed<PlayerVisuals> changed) {
        Color newColour = changed.Behaviour.PlayerColour;

        changed.Behaviour.ChangeColour(newColour);
        changed.Behaviour.ChangeDestroyedParticleEffectColour(newColour);
    }

    private void ChangeDestroyedParticleEffectColour(Color newColour) {
        var mainParticleModule = GetDestroyedParticleEffect.main;
        mainParticleModule.startColor = newColour;
    }

    private void ChangeColour(Color newColour) {
        ForEachMeshRenderer(ChangeMaterialColourTo(newColour));
    }

    private Action<MeshRenderer> ChangeMaterialColourTo(Color newColour) {
        return ((MeshRenderer renderer) => {
            renderer.material.color = newColour;
            renderer.material.SetColor(EMISSION_COLOR_PROPERTY_NAME, newColour);
        });
    }

    private void ForEachMeshRenderer(Action<MeshRenderer> action) {
        foreach(MeshRenderer element in _meshRenderers) {
            action(element);
        }
    }
}
