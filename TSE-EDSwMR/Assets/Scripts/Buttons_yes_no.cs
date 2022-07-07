using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons_yes_no : MonoBehaviour
{
    public POV_1 pov_script;

    public DataHandlerPOV dataHandlerPOV;


    // TODO: are these yes no prefabs needed?
    public GameObject yes_prefab;
    public GameObject no_prefab;

    private GameObject yes_button;
    private GameObject no_button;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // TODO: how to know if yes or no 
    /*
     * 
     */
    private void Clicked()
    {
        //yes_button.SetActive(false);
        pov_script.DisplayNextIcon();

    }

    public void Yes_Clicked()
    {

        dataHandlerPOV.UpdateDecisions(true);

        pov_script.DisplayNextIcon();
    }

    public void No_Clicked()
    {
        dataHandlerPOV.UpdateDecisions(false);

        pov_script.DisplayNextIcon();
    }





}
