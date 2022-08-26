using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POV_DecisionTree : MonoBehaviour
{
    [SerializeField] ToolTip nodeYes;
    [SerializeField] ToolTip nodeNo;
    [SerializeField] GameObject root;
    [SerializeField] GameObject innerNode;




    [SerializeField] GameObject[] parentNodes;
    [SerializeField] Material highlightingMaterial;
    [SerializeField] Material normalMaterial;


    [SerializeField] GameObject exampleDatapoint;
    public GameObject[] nodesExample;

    public int indexInTreeExampleLeave;

    private DataHandlerPOV dataHandler;
    private GameObject example;

    private ToolTip[] leaves;  // leaves are in same order as parent nodes so right now: [2,1,4,3] in day order 

    private bool[] decisions;



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
        leaves = new ToolTip[parentNodes.Length];
       
        for (int i = 0; i < parentNodes.Length; i++)
        {
            GameObject parentNode = parentNodes[i];
            leaves[i] = InstantiateNode(decisions[i], parentNode);

            // Positioning and line 
            leaves[i].transform.localPosition = new Vector3(0, (float)-0.2, 0);
            ToolTipConnector con = leaves[i].GetComponent<ToolTipConnector>();
            con.Target = parentNode;

        }

    }




    private ToolTip InstantiateNode(bool decision, GameObject parent)
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

    public void InstantiateExampleDatapoint()
    {
        example = GameObject.Instantiate(exampleDatapoint, gameObject.transform);
        example.transform.localPosition = new Vector3(0, 0.7f, 0);

    }

    public void DestroyExample()
    {
        GameObject.Destroy(example);
    }


    private void ChangeMaterial(GameObject node, Material mat)
    {
     
        GameObject tipBackground =  node.transform.Find("Pivot/ContentParent/TipBackground").gameObject;

        tipBackground.GetComponent<MeshRenderer>().material = mat;

    }

    public void HighlightNode(GameObject node)
    {
        ChangeMaterial(node, highlightingMaterial);
    } 
    public void RemoveHighlightNode(GameObject node)
    {
        ChangeMaterial(node, normalMaterial);
    }

    public void HighlightInnerNode()
    {
        ChangeMaterial(innerNode, highlightingMaterial);
    }

    public void HighlightRoot()
    {
        ChangeMaterial(root, highlightingMaterial);
    }


    public void RemoveHighlightInnerNode()
    {
        ChangeMaterial(innerNode, normalMaterial);
    }
    public void RemoveHighlightRoot()
    {
        ChangeMaterial(root, normalMaterial);
    }


    public void HighlightLeave(int leave_index_in_tree)
    {
        // leaves are in same order as parents 2,1,4,3 -> in array: 1,0,3,2
        // highlight node for 

        int[] match_index = { 1,0,3,2 };

        ToolTip correctLeave = leaves[match_index[leave_index_in_tree]];

        correctLeave.transform.localPosition += new Vector3(0, 0, -0.05f);
        correctLeave.transform.localScale += new Vector3(0.5f, 0.5f, 0);


    }

    public void RemoveHighlightLeave(int leave_index_in_tree)
    {
        int[] match_index = { 1, 0, 3, 2 };

        ToolTip correctLeave = leaves[match_index[leave_index_in_tree]];

        correctLeave.transform.localPosition -= new Vector3(0, 0, -0.05f);
        correctLeave.transform.localScale -= new Vector3(0.5f, 0.5f, 0);
    }



}
