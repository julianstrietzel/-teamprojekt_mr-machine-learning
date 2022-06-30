using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandlerPOV : MonoBehaviour
{

    private static bool[] decisions;
    private static int NUMBER_OF_DECISIONS = 4;

    // Start is called before the first frame update
    void Start()
    {
        decisions = new bool[NUMBER_OF_DECISIONS];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDecisions(int decision_nr, bool decision_YN)
    {
        decisions[decision_nr] = decision_YN;
    }

    public bool[] GetDecisions()
    {
        return decisions;
    }

}
