using System.Collections;
using UnityEngine;

public class MaterialColourInterpolator : MonoBehaviour {

    // Property name may change based on material shader used. Assuming default is used.
    private const string EMISSION_COLOR_PROPERTY_NAME = "_EmissionColor";

    [SerializeField] private Material _originalMaterial;
    [SerializeField] private Renderer[] _renderersToOverrideMaterial;
    [SerializeField] private Color[] _coloursToUse;
    [SerializeField] private float _transitionSeconds = 1f;

    private Material _copyMaterial;

    private int _currentColorIndex = 0;

    private void Start() {
        _copyMaterial = new Material(_originalMaterial);

        foreach (Renderer renderer in _renderersToOverrideMaterial) {
            renderer.material = _copyMaterial;
        }

        StartCoroutine(InterpolateColoursCoroutine());
    }

    private IEnumerator InterpolateColoursCoroutine() {
        while (true) {
            Color startColour = _copyMaterial.GetColor(EMISSION_COLOR_PROPERTY_NAME);
            Color targetColour = _coloursToUse[_currentColorIndex];
            float elapsedTime = 0f;

            Color colorToSet = Color.white;

            while (elapsedTime < _transitionSeconds) {
                colorToSet = Color.Lerp(startColour, targetColour, elapsedTime / _transitionSeconds);
                _copyMaterial.SetColor(EMISSION_COLOR_PROPERTY_NAME, colorToSet);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _currentColorIndex = (_currentColorIndex + 1) % _coloursToUse.Length;
        }
    }
}
