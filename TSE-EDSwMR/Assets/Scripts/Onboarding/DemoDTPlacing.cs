using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class for demo DT Placing in the Onboarding
/// On InitPlacementDemo() this activates and the surfacemagnetism follower to explain the placement in the beginning of M2,3,4
/// On CloseDemo() this calls the next part of Introduction: HandmenuIntro
/// </summary>
public class DemoDTPlacing : MonoBehaviour
{
    public GameObject frame_prefab;
    public GameObject button_prefab;

    // Declaration of public AudioHandlerScript 
    public AudioHandlerScript audioScript;

    // Declaration of GameObject used for dialog (gestrure control) and all needed string constants
    public GameObject dialog_prefab;
    private const string DIALOG_TITLE = "Gesture Control Introduction";
    private const string DIALOG_TEXT = "Excellent, you locked the frame. \nIn the upcoming modules you have the possibility to use the 'reposition' button in the hand menu to unlock the frame and try again. \nNow you have learned all interaction possibilities and are ready to go.\nYou can now put the paper frame back in the box and take out the tennis ball, afterwards you can put the box away.";

    protected GameObject place_button;
    protected UnityEvent place_button_pressed;

    public OnboardingOutro outro;

    private void Dissable_Following()
    {
        gameObject.transform.GetComponentInParent<Microsoft.MixedReality.Toolkit.Utilities.Solvers.SolverHandler>().enabled = false;
        place_button.SetActive(false);
    }

    public void InitPlacementDemo(DialogResult res)
    {
        InitPlacementDemo();
        outro.SetHint("Placement Hint", "The Decision Tree will follow your gaze. \nTo place it permanently click the button in front of you.");
    }

    /// <summary>
    /// use this method to intit the demo
    /// </summary>
    public void InitPlacementDemo()
    {
        transform.parent.gameObject.SetActive(true);
        GameObject root = Instantiate(frame_prefab, gameObject.transform);

        place_button = Instantiate(button_prefab, gameObject.transform.parent.parent);
        place_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Placed correctly?";

        place_button_pressed = EnableFollowing();
        place_button_pressed.AddListener(Dissable_Following);
        place_button_pressed.AddListener(SumUpDemo);
    }

    private UnityEvent EnableFollowing()
    {
        gameObject.transform.GetComponentInParent<Microsoft.MixedReality.Toolkit.Utilities.Solvers.SolverHandler>().enabled = true;
        gameObject.transform.GetComponentInParent<Microsoft.MixedReality.Toolkit.Utilities.Solvers.SurfaceMagnetism>().enabled = true;
        place_button.SetActive(true);

        return place_button.GetComponent<PressableButtonHoloLens2>().ButtonPressed;
    }

    /// <summary>
    /// This method opens the summary dialog which completes the GestureControls.
    /// </summary>
    public void SumUpDemo()
    {
        // plays the audio file of the gestrue control section
        outro.ReadDialog(); // "Excellent, you locked the frame. In the upcoming modules you have the possibility to use the 'reposition' button to unlock the frame and try again. Now you have learned all interaction possibilities and are ready to go." 


        // opens the summary dialog for positioning and locking-in the virtual frame
        Dialog dialog = Dialog.Open(dialog_prefab, DialogButtonType.OK, DIALOG_TITLE, DIALOG_TEXT, true);

        // calls CloseDemo method after closing the dioalog field
        dialog.OnClosed += CloseDemo;
    }

    /// <summary>
    /// This method closes the demo (frame) and starts the next part of onboarding (hand menu introduction).
    /// </summary>
    /// <param name="res"></param>
    public void CloseDemo(DialogResult res)
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        outro.HandMenuIntroduction();
    }
}
