using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the Hint and texts of the hints in the hand menu.
/// </summary>
public class TextAndHintsPOV : MonoBehaviour
{

    public GameObject hintPrefab;
    public GameObject informationPanelPrefab;

    private string hint_choosing_phase = "If you think the weather will be suited for a tennis day, click the 'Yes' button otherwise click 'No'.";
    private string hint_explanation = "The Nodes on the top are called 'Root', the ones in the middle 'Inner node' and on the bottom are the 'leaves'.\n\n"
                                + "In a question you decide on the value (e.g. rainy) of an attribute (e.g. Outlook).\n\n"
                                + "A decision tree can be built with machine learning. To automize complex, but systematic decisions, so a machine can help you or decide for you.\n\n"
                                + "It's recommended to continue in the next module and find out how to learn from data and how a computer builts a decision tree."
                                + "However, you can get back to the main menu using the 'back to menu' button in the hand menu";


    private void Hint(string message)
    {
        hintPrefab.SetActive(true);
        Dialog.Open(hintPrefab, DialogButtonType.OK, "Hint", message, true);
    }

    public void HintChoosingPhase()
    {
        Hint(hint_choosing_phase);
    }
    public void HintExplanationPhase()
    {
        Hint(hint_explanation);
    }

}
