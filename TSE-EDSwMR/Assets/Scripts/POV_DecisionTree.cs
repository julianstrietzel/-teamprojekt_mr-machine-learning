using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POV_DecisionTree : MonoBehaviour
{
    [SerializeField] ToolTip nodeYes;
    [SerializeField] ToolTip nodeNo;

    [SerializeField] GameObject[] parentNodes;

    private DataHandlerPOV dataHandler;

    private bool[] decisions;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitiateTree(DataHandlerPOV data)
    {
        SetDecisions(data);
        DisplayDecisionNodes();
    }
    private void SetDecisions(DataHandlerPOV data)
    {
        dataHandler = data;
        decisions = dataHandler.GetFinalDecisions();
    }

    private void DisplayDecisionNodes()
    {
        for (int i = 0; i < parentNodes.Length; i++)
        {
            GameObject parentNode = parentNodes[i];
            ToolTip leave = InstatitiateNode(decisions[i], parentNode);
    
            // Positioning and line 
            leave.transform.localPosition = new Vector3(0, (float)-0.2, 0);
            ToolTipConnector con = leave.GetComponent<ToolTipConnector>();
            con.Target = parentNode;

        }

    }



    private ToolTip InstatitiateNode(bool decision, GameObject parent)
    {
        ToolTip leave;

        if (decision) 
        {
            leave = Instantiate(nodeYes, parent.transform);

        }
        else
        {
            leave = Instantiate(nodeNo, parent.transform);
        }

        return leave;
    }




}
