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



    private DataHandlerPOV dataHandler;
    private GameObject example;

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
        for (int i = 0; i < parentNodes.Length; i++)
        {
            GameObject parentNode = parentNodes[i];
            ToolTip leave = InstantiateNode(decisions[i], parentNode);
    
            // Positioning and line 
            leave.transform.localPosition = new Vector3(0, (float)-0.2, 0);
            ToolTipConnector con = leave.GetComponent<ToolTipConnector>();
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
        example.transform.localPosition = new Vector3(-0.03f, 0.6f, 0);

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






}
