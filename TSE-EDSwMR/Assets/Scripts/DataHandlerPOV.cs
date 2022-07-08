using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandlerPOV : MonoBehaviour
{
    //private static int AMOUNT_DECISIONS = 4;

    //private bool[] decisions = new bool[AMOUNT_DECISIONS];

    private List<bool> decisionList = new List<bool>();


    //private int decision_nr;

    // Start is called before the first frame update
    void Start()
    {
        //decisions = new bool[AMOUNT_DECISIONS];
    }

    // Update is called once per frame
    void Update()
    {
        
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


}
