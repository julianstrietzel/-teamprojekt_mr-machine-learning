using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScriptPOV : MonoBehaviour
{

    public PanelIconsPOV panelIcons;

    public DataHandlerPOV dataHandler;

    private int day = 1;
    private int icon_nr = 0;
    private bool finishedGame = false;



    //private static int AMOUNT_ICONS = 3;

    //// IDEA: putting the icons in a list would be more flexible. But harder to see the order, additional its also harder to say which position it should go to
    //// what if the position is also set in unity?


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("in State Script");        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCurrentState()
    {
        if (icon_nr >= PanelIconsPOV.AMOUNT_ICONS)
        {
            icon_nr = 0;
            day++;
        }

        if (day > PanelIconsPOV.AMOUNT_DAYS)
        {
            finishedGame = true;
            Debug.Log(" set finishedGame ==: " + finishedGame);

        }

        else
        {
            icon_nr++;
        }

    }

    // TODO: Test day change, implement end of game 
    // TODO: add Kai speech
    private void Clicked()
    {

        Debug.Log("Clicked- Day: " + day + " icon_nr: " + icon_nr);

        UpdateCurrentState();

        Debug.Log("after update- Day: " + day + " icon_nr: " + icon_nr);

        Debug.Log("finished?: " + finishedGame);

        if (finishedGame && panelIcons != null)
        {
            // TODO Destroy Panel and Buttons
            // TODO Kai talks and introduces DT                         
            panelIcons.DestroyPanelAfterGameFinish();
        }
        else
        {
            panelIcons.DisplayNextIcon(day, icon_nr);

        }

    }

    public void Yes_Clicked()
    {

        dataHandler.AddDecision(true);

        Clicked();
    }

    public void No_Clicked()
    {
        dataHandler.AddDecision(false);

        Clicked();
    }

    public bool GetFinishedGame()
    {
        return finishedGame;
    }


}
