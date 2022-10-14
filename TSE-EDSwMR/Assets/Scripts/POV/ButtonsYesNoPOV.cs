using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script informs the State Script, which handels the current state of the POV Game, which button was clicked.
/// And destroys the buttons after the game is finished.
/// </summary>
public class ButtonsYesNoPOV : MonoBehaviour
{
    public GameObject buttons;

    public StateScriptPOV stateScript;

    // Start is called before the first frame update
    void Start()
    {
        buttons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        DestroyButtonsAfterGameFinished();
        //activates buttons after the intro from the bot is over
        if (buttons.activeInHierarchy == false && !stateScript.GetIntroIsPlaying())
        {
            buttons.SetActive(true);

        }

    }


    /// <summary>
	/// informs the state script, that yes was clicked
	/// </summary>
    public void Yes_Clicked()
    {

        stateScript.Yes_Clicked();

    }

    /// <summary>
	/// informs the state script, that no was clicked
	/// </summary>
    public void No_Clicked()
    {

        stateScript.No_Clicked();

    }

    /// <summary>
    /// destroys the button game object in the scene, after the decision part of the Module 1 is finished
    /// </summary>
    private void DestroyButtonsAfterGameFinished()
    {
        if (stateScript.GetFinishedGame())
        {
            Destroy(gameObject);

        }
    }

}
