using Fusion;
using System;
using System.Collections;
using UnityEngine;

public class PlayerVisuals : NetworkBehaviour {
    
    // Property name may change based on material shader used. Assuming default is used.
    private const string EMISSION_COLOR_PROPERTY_NAME = "_EmissionColor";
    private const float FLASH_SPEED = 5f;

    [SerializeField] private MeshRenderer[] _meshRenderers;
    [SerializeField] private ParticleSystem _destroyedSFX;

    [Header("Flash when Invincible")]
    [SerializeField] private PlayerInvincibility _playerInvincibility;

    private Coroutine _invincibleFlashCoroutine;

    public ParticleSystem GetDestroyedParticleEffect => _destroyedSFX;

    [Networked(OnChanged = nameof(OnPlayerColourChanged))]
    public Color PlayerColour { get; private set; }

    private void OnEnable() {
        _playerInvincibility.OnInvincibilityChanged -= WhenPlayerInvincibilityChanged;
        _playerInvincibility.OnInvincibilityChanged += WhenPlayerInvincibilityChanged;
    }

    private void OnDisable() {
        _playerInvincibility.OnInvincibilityChanged -= WhenPlayerInvincibilityChanged;
    }

    private void WhenPlayerInvincibilityChanged(bool isNowInvincible) {
        if (isNowInvincible) {
            StartInvincibleFlashEffect();
        }
        else {
            StopInvincibleFlashEffect();
        }
    }

    public override void Spawned() {
        PlayerColour = PlayerColourManager.GetPlayerColour(Object.InputAuthority, Runner);
    }

    public void DestroyedEffect() {
        HidePlayer();
        _destroyedSFX.Play();
    }

    public void StartInvincibleFlashEffect() {
        StopInvincibleFlashEffect();
        
        _invincibleFlashCoroutine = StartCoroutine(FlashCoroutine());
    }

    public void StopInvincibleFlashEffect() {
        _invincibleFlashCoroutine = null;
        
        ForEachMeshRenderer(SetRendererAlpha(1f));
    }

    private IEnumerator FlashCoroutine() {
        while (_playerInvincibility.IsInvincible) {
            float alpha = Mathf.PingPong(Time.time * FLASH_SPEED, 1f);
            ForEachMeshRenderer(SetRendererAlpha(alpha));
            yield return null;
        }

        ForEachMeshRenderer(SetRendererAlpha(1f));
    }

    private Action<MeshRenderer> SetRendererAlpha(float alphaValue) {
        return ((MeshRenderer renderer) => {
            Color changedAlphaColour = renderer.material.color;
            changedAlphaColour.a = alphaValue;
            renderer.material.color = changedAlphaColour;
        });
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
