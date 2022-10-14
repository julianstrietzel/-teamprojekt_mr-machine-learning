using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handels the information which button was clicked (yes or no) <see cref="ButtonsYesNoPOV"/>
/// it gets the information from <see cref="StateScriptPOV"/> and provides it to <see cref="POV_DecisionTree"/> to display the correct leave node.
/// </summary>
public class DataHandlerPOV : MonoBehaviour
{

    [System.Obsolete("was used to save each decision the user made")]
    private List<bool> decisionList = new List<bool>();

    // only saves the final decision used for the leave nodes in the tree
    private bool[] finalDecisionOfDay = new bool[StateScriptPOV.AMOUNT_DAYS];

    /// <summary>
	/// Set final decision for the tree
	/// </summary>
	/// <param name="day">the example number</param>
	/// <param name="decision">yes or no</param>
    public void SetFinalDecision(int day, bool decision)
    {
        finalDecisionOfDay[day - 1] = decision;
    }

    [System.Obsolete("for saving every decision")]
    public void AddDecision(bool decision)
    {
        decisionList.Add(decision);
    }

    [System.Obsolete("for returning every decision")]
    public List<bool> GetDecisions()
    {
        return decisionList;
    }

    /// <summary>
	/// provides the information for the leaves of the decision tree.
	/// <returns>the array with the decision the user made for the example days
	/// </summary></returns>  
    public bool[] GetFinalDecisions()
    {

        return finalDecisionOfDay;
    }


}
