using Fusion;
using System;
using UnityEngine;

public class PlayerVisuals : SimulationBehaviour {

    [SerializeField] private MeshRenderer[] _meshRenderers;

    private void Update() {
        //if(Input.GetKeyDown(KeyCode.C)) {
        //    ChangeColour(new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));
        //}
    }

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
