using UnityEngine;

[CreateAssetMenu(fileName = "ColourList", menuName = "ScriptableObjects/ColourList")]
public class ColourList : ScriptableObject {
    [SerializeField] public Color[] Colours;
}
