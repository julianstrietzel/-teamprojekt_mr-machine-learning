using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsYesNoPOV : MonoBehaviour
{


    public StateScriptPOV stateScript;


    // TODO: are these yes no prefabs needed?
    //public GameObject yes_prefab;
    //public GameObject no_prefab;

    //private GameObject yes_button;
    //private GameObject no_button;


    // Start is called before the first frame update
    void Start()
    {

        // TODO: click button does not work, function does
       /* Debug.Log("ButtonsYesNo");
        Yes_Clicked();
        No_Clicked();*/
    }

    // Update is called once per frame
    void Update()
    {
        
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





}
