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
    private const string DIALOG_GESTURE_RECTANGLE_TEXT = "Great, than let's start with the basics. Do you see the rectangle in the box? \nPlease, place it centrally at the back edge on the table. \n\nAre you finished?";
    private const string DIALOG_GESTURE_FRAME_TITLE = "Gesture Control Introduction";
    private const string DIALOG_GESTURE_FRAME_TEXT = "Following this explanation, you should see a frame. Move your head to position the frame on the rectangle. Press the button to lock the position. \n\nClose this dialog first!";
    private const string DIALOG_HANDMENU_TITLE = "Handmenu Introduction";
    private const string DIALOG_HANDMENU_TEXT = "Perfect, now let's continue with the last part of the onboarding module. \nPlease open your hand menu. To do this, look at the palm of your right hand and press the button that appears on your wrist. \nTo complete the onboarding module click the 'Back to menu' button in the hand menu.";
    private const string DIALOG_HINT_TITLE = "Hint Explanation";
    private const string DIALOG_HINT_TEXT = "Super, you clicked the 'hint' button. \nAs you know, this button displays some additional information in the respective modules if you get stuck.";
    private const string DIALOG_REPOSITION_TITLE = "Reposition Explanation";
    private const string DIALOG_REPOSITION_TEXT = "Great, you clicked the button. \nAs you know, this button realigns your Decision Tree in the respective modules if you have too little space.";

    // Declaration of private string variables to show the user the suitable hint dialog
    private string hint_message;
    private string hint_title;

    // Declaration of private boolean variable to indicate if the HandMenuInroduction has allready started
    private bool handMenuInteraction = false;

    //Declaration of private int variable to count the audio clips
    private int clipNumber = 0;

    // Declaration of private int variable to indicate the audio clip number of special audios (first audio of the handler, hand menu audios)
    private const int clipNumberHint = 4;
    private const int clipNumberReposition = 5;

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
    /// This method plays the audio files and increases after that the variable clipNumber.
    /// </summary>
    public void ReadDialog()
    {
        audioScript.PlayAudioClipNr(this.clipNumber);
        this.clipNumber++;
    }

    /// <summary>
    /// This method plays the audio file with the given clipNumber.
    /// </summary>
    /// <param name="clipNumber"> of the audio file the method should play </param>
    public void ReadDialog(int clipNumber)
    {
        audioScript.PlayAudioClipNr(clipNumber);
    }

    /// <summary>
    /// This method opens the explanation dialog which introduces the GestureControls (first dialog).
    /// </summary>
    public void GestureControlIntroduction()
    {
        // plays the audio files of the gesture control section
        ReadDialog(); // "Great, than let's start with the basics. Do you see the rectangle in the box? Please, place it centrally at the back edge on the table."
        // TODO: Luca build coroutine to stop audio for a few seconds and continue with other audio
        // ReadDialog(); // "Are you finished?"

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

        // plays the audio file of the gesture control section
        ReadDialog(); // "Following this explanation, you should see a frame. Move your head to position the frame on the rectangle. Press the button to lock the position."

        // opens the explanation dialog for positioning and locking-in the virtual frame
        Dialog.Open(dialogPrefab, DialogButtonType.OK, DIALOG_GESTURE_FRAME_TITLE, DIALOG_GESTURE_FRAME_TEXT, true).OnClosed += demoTreeScript.InitPlacementDemo;

    }

    /// <summary>
    /// This method opens the explanation dialog which introduces the HandMenu and its funcionality.
    /// </summary>
    public void HandMenuIntroduction()
    {
        // plays the audio file of the hand menu section
        ReadDialog(); // "Perfect, now let's continue with the last part of the onboarding module. Please open your hand menu. To do this, look at the palm of your right hand and press the button that appears on your wrist. To complete the onboarding module click the 'Back to menu' button in the hand menu."

        // opens the explanation dialog
        Dialog explanationDialog = Dialog.Open(dialogPrefab, DialogButtonType.OK, DIALOG_HANDMENU_TITLE, DIALOG_HANDMENU_TEXT, true);

        // set handMenuInteraction to 'true' to indicate that the hand menu introduction started
        handMenuInteraction = true;
    }

    /// <summary>
    /// This method shows an explanation dialog if the user presses the "hint" button.
    /// </summary>
    public void HandMenuHint()
    {
        // plays the audio file of the 'hint' button explanation
        ReadDialog(clipNumberHint); // "Super, you clicked the 'hint' button. As you know, this button displays some additional information in the respective modules if you get stuck."

        // opens the explanation dialog
        Dialog hintDialog = Dialog.Open(dialogPrefab, DialogButtonType.OK, DIALOG_HINT_TITLE, DIALOG_HINT_TEXT, true);
    }

    /// <summary>
    /// This method shows an explanation dialog if the user presses the "reposition" button.
    /// </summary>
    public void HandMenuReposition()
    {
        // plays the audio file of the 'reposition' button explanation
        ReadDialog(clipNumberReposition); // "Great, you clicked the button. As you know, this button realigns your Decision Tree in the respective modules if you have too little space." 

        // opens the explanation dialog
        Dialog repositionDialog = Dialog.Open(dialogPrefab, DialogButtonType.OK, DIALOG_REPOSITION_TITLE, DIALOG_REPOSITION_TEXT, true);
    }

    /// <summary>
    /// This method shows the hint dialog if the user needs help.
    /// </summary>
    public void Hint()
    {
        if (hint_title == null) return;
        else if (handMenuInteraction)
        {
            // calls the special HandMenuHint method for the 'hand menu introduction' section of onboarding
            HandMenuHint();
        } else {
            Dialog.Open(dialogPrefab, DialogButtonType.OK, hint_title, hint_message, true);
        }
    }

    /// <summary>
    /// This method updates the hint dialog with the given parameters.
    /// </summary>
    /// <param name="title">< string, of the dialog title /param> 
    /// <param name="message">< string, of the dialog message /param>
    public void SetHint(string title, string message)
    {
        hint_title = title;
        hint_message = message;
    }
}
