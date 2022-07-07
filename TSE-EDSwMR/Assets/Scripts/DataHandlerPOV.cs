using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandlerPOV : MonoBehaviour
{

    private static bool[] decisions;
    private static int NUMBER_OF_DECISIONS = 4;

    public int number_of_decisions;

    private int decision_nr;

    // Start is called before the first frame update
    void Start()
    {
        decisions = new bool[number_of_decisions];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDecisions(bool decision_YN)
    {
        decisions[decision_nr] = decision_YN;

        decision_nr++;
    }

    public bool[] GetDecisions()
    {
        return decisions;
    }

}
