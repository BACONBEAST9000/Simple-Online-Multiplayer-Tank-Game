using Fusion;
using System;
using UnityEngine;

public class PlayerVisuals : SimulationBehaviour {

    [SerializeField] private MeshRenderer[] _meshRenderers;
    [SerializeField] private ParticleSystem _destroyedSFX;

    private bool _isShown = true;

    private void OnEnable() {
        Player.OnPlayerDestroyed -= WhenPlayerDestroyed;
        Player.OnPlayerDestroyed += WhenPlayerDestroyed;

        Player.OnPlayerRespawned -= WhenPlayerRespawns;
        Player.OnPlayerRespawned += WhenPlayerRespawns;
    }


    private void OnDisable() {
        Player.OnPlayerDestroyed -= WhenPlayerDestroyed;
        Player.OnPlayerRespawned -= WhenPlayerRespawns;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.C)) {
            ChangeColour(new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            _isShown = !_isShown;
            TogglePlayerVisibility(_isShown);
        }
    }
    
    private void WhenPlayerDestroyed(PlayerRef playerRef) {
        //if (playerRef != Object.InputAuthority) return;

        ////print($"Player ID: {playerRef.PlayerId} got hit! Should now disappear!");
        //DestroyedEffect();
    }
    
    private void WhenPlayerRespawns(PlayerRef playerRef) {
        //if (playerRef != Object.InputAuthority) return;

        ////print($"Player ID: {playerRef.PlayerId} respawned! Should now appear!");
        //ShowPlayer();
    }

    public void DestroyedEffect() {
        HidePlayer();
        _destroyedSFX.Play();
    }

    public void ShowPlayer() => TogglePlayerVisibility(true);
    public void HidePlayer() => TogglePlayerVisibility(false);
    public void TogglePlayerVisibility(bool isVisible) => ForEachMeshRenderer((MeshRenderer renderer) => renderer.enabled = isVisible);

    private void ChangeColour(Color newColour) {
        ForEachMeshRenderer(ChangeMaterialColourTo(newColour));
    }

    private Action<MeshRenderer> ChangeMaterialColourTo(Color newColour) {
        return (MeshRenderer renderer) => renderer.material.color = newColour;
    }

    private void ForEachMeshRenderer(Action<MeshRenderer> action) {
        foreach(MeshRenderer element in _meshRenderers) {
            action(element);
        }
    }
}
