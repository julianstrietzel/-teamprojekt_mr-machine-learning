using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandlerPOV : MonoBehaviour
{


    private List<bool> decisionList = new List<bool>();

    private bool[] finalDecisionOfDay = new bool[StateScriptPOV.AMOUNT_DAYS];


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFinalDecision(int day, bool decision)
    {
        finalDecisionOfDay[day - 1] = decision;
    }

    public void AddDecision(bool decision)
    {
        decisionList.Add(decision);
    }


    //public void UpdateDecisions(bool decision_YN)
    //{
    //    decisions[decision_nr] = decision_YN;

    //    decision_nr++;
    //}

    public List<bool> GetDecisions()
    {
        return decisionList;
    }

    public bool[] GetFinalDecisions()
    {
        Debug.Log("Getting final decisions");
        foreach (bool decision in finalDecisionOfDay)
        {
            Debug.Log("Final decision is: " + decision);
        }
        return finalDecisionOfDay;
    }


}
