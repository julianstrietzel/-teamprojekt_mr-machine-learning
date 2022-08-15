using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsYesNoPOV : MonoBehaviour
{


    public StateScriptPOV stateScript;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DestroyButtonsAfterGameFinished();
    }
    // TODO: Differnt class for click
    /*
     * Display next icon
     * Start new audio
     * 
     */
    //private void Clicked()
    //{
    //    //yes_button.SetActive(false);
    //    pov_script.DisplayNextIcon();

    //}

    public void Yes_Clicked()
    {
        stateScript.Yes_Clicked();

    }

    public void No_Clicked()
    {
        stateScript.No_Clicked();

    }

    private void DestroyButtonsAfterGameFinished()
    {
        if (stateScript.GetFinishedGame())
        {
            Destroy(gameObject);

        }
    }



}
