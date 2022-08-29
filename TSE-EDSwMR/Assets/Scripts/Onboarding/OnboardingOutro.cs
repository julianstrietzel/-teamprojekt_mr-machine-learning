using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingOutro : MonoBehaviour
{
    // Declaration of the public GameObject for the hand menu in the hand menu section of the onboarding module
    public GameObject handmenu;

    // Declaration of public AudioHandlerScript 
    public AudioHandlerScript audioScript;

    // Declaration of the public GameObject for the demoTree in the gesture control section of the onboarding module
    public GameObject demoTree;

    // Declaration of GameObject used for dialogs (gestrure control, hand menu) and all needed string constants
    public GameObject dialogPrefab;
    private const string DIALOG_GESTURE_RECTANGLE_TITLE = "Gesture Control Introduction";
    private const string DIALOG_GESTURE_RECTANGLE_TEXT = "Great, than let's start with the basics. Do you see the rectangle in the box? \nPlease, place it centrally at the back edge on the table. \nAre you finished?";
    private const string DIALOG_GESTURE_FRAME_TITLE = "Gesture Control Introduction";
    private const string DIALOG_GESTURE_FRAME_TEXT = "Following this explanation, you should see a frame. Move your head to position the frame on the rectangle. Press the button to lock the position.";
    private const string DIALOG_HANDMENU_TITLE = "Handmenu Introduction";
    private const string DIALOG_HANDMENU_TEXT = "Perfect, now let's continue with the last part of the onboarding module. \nPlease open your hand menu. To do this, look at the palm of your right hand and press the button that appears on your wrist. \nTo complete the onboarding module click the 'Back to menu' button in the hand menu.";
    private const string DIALOG_HINT_TITLE = "Hint Explanation";
    private const string DIALOG_HINT_TEXT = "Super, you clicked the 'hint' button. \nAs you know, this button displays some additional information in the respective modules if you get stuck.";
    private const string DIALOG_REPOSITION_TITLE = "Reposition Explanation";
    private const string DIALOG_REPOSITION_TEXT = "Great, you clicked the button. \nAs you know, this button realigns your Decision Tree in the respective modules if you have too little space.";


    private string hint_message;
    private string hint_title;
    // Start is called before the first frame update
    void Start()
    {
        SetHint("Hint", "Cool you found the hint button. If you should ever get stuck in the learning experience, you'll find useful hints here.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This method opens the explanation dialog which introduces the GestureControls (first dialog).
    /// </summary>
    public void GestureControlIntroduction()
    {
        // TODO: Luca put in the right clip number
        // TODO: Luca build coroutine to stop audio for a few seconds
        // plays the audio files of the gesture control section
        audioScript.PlayAudioClipNr(0); // "Great, than let's start with the basics. Do you see the rectangle in the box? Please, place it centrally at the back edge on the table."
        audioScript.PlayAudioClipNr(1); // "Are you finished?"

        // opens the explanation dialog for the premade rectangle
        Dialog explanationDialog = Dialog.Open(dialogPrefab, DialogButtonType.Yes, DIALOG_GESTURE_RECTANGLE_TITLE, DIALOG_GESTURE_RECTANGLE_TEXT, true);

        // calls OnClose method after closing the dioalog field
        explanationDialog.OnClosed += OnClose;
    }

    /// <summary>
    /// This method opens the explanation dialog which introduces the GestureControls (second dialog).
    /// </summary>
    /// <param name="res"></param>
    public void OnClose(DialogResult res)
    {
        // initialisation of demoTreeScript for placing the demoTree component in the onboarding module
        DemoDTPlacing demoTreeScript = demoTree.GetComponent<DemoDTPlacing>();
        demoTreeScript.outro = this;

        // TODO: Luca put in the right clip number
        // plays the audio file of the gesture control section
        audioScript.PlayAudioClipNr(2); // "Following this explanation, you should see a frame. Move your head to position the frame on the rectangle. Press the button to lock the position."

        // opens the explanation dialog for positioning and locking-in the virtual frame
        Dialog.Open(dialogPrefab, DialogButtonType.OK, DIALOG_GESTURE_FRAME_TITLE, DIALOG_GESTURE_FRAME_TEXT, true).OnClosed += demoTreeScript.InitPlacementDemo;

    }

    /// <summary>
    /// This method opens the explanation dialog which introduces the HandMenu and its funcionality.
    /// </summary>
    public void HandMenuIntroduction()
    {
        // TODO: Luca put in the right clip number
        // plays the audio file of the hand menu section
        audioScript.PlayAudioClipNr(3); // "Perfect, now let's continue with the last part of the onboarding module. Please open your hand menu. To do this, look at the palm of your right hand and press the button that appears on your wrist. To complete the onboarding module click the 'Back to menu' button in the hand menu."

        // opens the explanation dialog
        Dialog explanationDialog = Dialog.Open(dialogPrefab, DialogButtonType.OK, DIALOG_HANDMENU_TITLE, DIALOG_HANDMENU_TEXT, true);
    }

    /// <summary>
    /// This method shows an explanation dialog if the user presses the "hint" button.
    /// </summary>
    public void HandMenuHint()
    {
        // TODO: Luca put in the right clip number
        // plays the audio file of the 'hint' button explanation
        audioScript.PlayAudioClipNr(4); // "Super, you clicked the 'hint' button. As you know, this button displays some additional information in the respective modules if you get stuck."

        // opens the explanation dialog
        Dialog hintDialog = Dialog.Open(dialogPrefab, DialogButtonType.OK, DIALOG_HINT_TITLE, DIALOG_HINT_TEXT, true);
    }

    /// <summary>
    /// This method shows an explanation dialog if the user presses the "reposition" button.
    /// </summary>
    public void HandMenuReposition()
    {
        // TODO: Luca put in the right clip number
        // plays the audio file of the 'reposition' button explanation
        audioScript.PlayAudioClipNr(5); // "Great, you clicked the button. As you know, this button realigns your Decision Tree in the respective modules if you have too little space." 

        // opens the explanation dialog
        Dialog repositionDialog = Dialog.Open(dialogPrefab, DialogButtonType.OK, DIALOG_REPOSITION_TITLE, DIALOG_REPOSITION_TEXT, true);
    }

    public void Hint()
    {
        if (hint_title == null) return;
        Dialog.Open(dialogPrefab, DialogButtonType.OK, hint_title, hint_message, true);
    }

    public void SetHint(string title, string message)
    {
        hint_title = title;
        hint_message = message;
    }

}
