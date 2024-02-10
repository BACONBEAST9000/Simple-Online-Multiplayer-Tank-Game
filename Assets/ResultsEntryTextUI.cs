using TMPro;
using UnityEngine;

public class ResultsEntryTextUI : MonoBehaviour {

    private static string[] _ordinalNumbers = { "1st", "2nd", "3rd", "4th" };
    private static float[] _fontSizes = { 72, 64, 56, 42 };

    [SerializeField] private TMP_Text _entryText;
    [SerializeField] private ColourList _resultsPositionColours;

    public void SetAndDisplayEntry(Player player, int positionIndex) {
        string ordinalNumberString = _ordinalNumbers[positionIndex];
        float fontSize = _fontSizes[positionIndex];
        Color textColour = _resultsPositionColours.Colours[positionIndex];

        _entryText.text = $"{ordinalNumberString} - {player.NickName}";
        _entryText.fontSize = fontSize;
        _entryText.color = textColour;
        
        _entryText.gameObject.SetActive(true);
    }
}
