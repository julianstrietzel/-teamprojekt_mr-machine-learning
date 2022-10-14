using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays the leave nodes in correct color and highlighting of the nodes for the animation 
/// </summary>
public class POV_DecisionTree : MonoBehaviour
{
    //ToolTips used for the yes and no leaves
    [SerializeField] ToolTip nodeYes;
    [SerializeField] ToolTip nodeNo;

    // the root of the tree
    [SerializeField] GameObject root;
    // the inner node which is highlighted through the animation
    [SerializeField] GameObject innerNode;
    // the lowest row of attributes where the leaves will be attached 
    [SerializeField] GameObject[] parentNodes;
    //Material used for the highlight in the animation
    [SerializeField] Material highlightingMaterial;
    // the material the nodes normally have
    [SerializeField] Material normalMaterial;

    //the example prefab which is displayed over the tree
    [SerializeField] GameObject exampleDatapoint;

    // the nodes that are highlighted when the example runs through the tree. Used by the state script
    public GameObject[] nodesExample;

    // the index for the leave the example ends up in to highlight it 
    public int indexInTreeExampleLeave;

    private GameObject example;

    private ToolTip[] leaves;  // leaves are in same order as parent nodes so right now: [2,1,4,3] in day order 

    private bool[] decisions;


    /// <summary>
	/// called to display the leave nodes with the tree
	/// gets the information about the decision for the leave nodes from <see cref="DataHandlerPOV"/>
	/// and initiates the display of the leaves
	/// </summary>
	/// <param name="data"></param>
    public void InitiateTree(DataHandlerPOV data)
    {
        SetDecisions(data);
        DisplayDecisionNodes();
    }

	// sets the decisions (yes/no) as provided by the given dataHandler
    private void SetDecisions(DataHandlerPOV data)
    {
       
        decisions = data.GetFinalDecisions();
    }

    /// <summary>
	/// Instatiates all the leave nodes, positions the under their parent node and connects the line to the parent
	/// </summary>
    private void DisplayDecisionNodes()
    {
        leaves = new ToolTip[parentNodes.Length];
       
        for (int i = 0; i < parentNodes.Length; i++)
        {
            GameObject parentNode = parentNodes[i];
            leaves[i] = InstantiateNode(decisions[i], parentNode);

            // Positioning and line 
            leaves[i].transform.localPosition = new Vector3(0, -0.2f, 0);
            ToolTipConnector con = leaves[i].GetComponent<ToolTipConnector>();
            con.Target = parentNode;

        }

    }



    /// <summary>
	/// Instantiates the nodes depending on yes or no decision, so which color/tooltip is used
	/// </summary>
	/// <param name="decision">yes or no depending on this the Tooltip is choosen for the correct color</param>
	/// <param name="parent">parent node of the leave, to which the line will be connected </param>
	/// <returns> returns the new leave node(ToolTip), as yes or no and a parent </returns>
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

    /// <summary>
	/// Instantiates the Example Datapoint that is run through the tree, positions the example 
	/// </summary>
    public void InstantiateExampleDatapoint()
    {
        example = GameObject.Instantiate(exampleDatapoint, gameObject.transform);
        example.transform.localPosition = new Vector3(0, 0.6f, 0);

    }

    /// <summary>
	/// destroy the example gameobject over the tree
	/// </summary>
    public void DestroyExample()
    {
        GameObject.Destroy(example);
    }

    /// <summary>
	/// changes the material/color of the given node
	/// </summary>
	/// <param name="node">node of which material is changed</param>
	/// <param name="mat">material the node is changed to</param>
    private void ChangeMaterial(GameObject node, Material mat)
    {
     
        GameObject tipBackground =  node.transform.Find("Pivot/ContentParent/TipBackground").gameObject;

        tipBackground.GetComponent<MeshRenderer>().material = mat;

    }

    /// <summary>
	/// highlights the provided node with the highlighting material given in unity
	/// </summary>
	/// <param name="node">the node that will be highlighted</param>
    public void HighlightNode(GameObject node)
    {
        ChangeMaterial(node, highlightingMaterial);
    }
    /// <summary>
	/// removes the highlight of the node by changing the color back to the normal material 
	/// </summary>
	/// <param name="node"></param>
    public void RemoveHighlightNode(GameObject node)
    {
        ChangeMaterial(node, normalMaterial);
    }

    /// <summary>
	/// Highlights the inner node which is set in unity to be highlighted
	/// </summary>
    public void HighlightInnerNode()
    {
        ChangeMaterial(innerNode, highlightingMaterial);
    }

    /// <summary>
	/// highlights the root node provided in unity
	/// </summary>
    public void HighlightRoot()
    {
        ChangeMaterial(root, highlightingMaterial);
    }

    /// <summary>
    /// highlights the root node provided in unity
    /// </summary>
    public void RemoveHighlightInnerNode()
    {
        ChangeMaterial(innerNode, normalMaterial);
    }


    /// <summary>
    /// removes the highlight of the inner node which is set in unity to be highlighted
    /// </summary>
    public void RemoveHighlightRoot()
    {
        ChangeMaterial(root, normalMaterial);
    }

    /// <summary>
	/// highlights the leave by increasing its size and bringing it closer to the user
	/// </summary>
	/// <param name="leave_index_in_tree">index of the leave from left to right from 0</param>
    public void HighlightLeave(int leave_index_in_tree)
    {
        // leaves are in same order as parents 2,1,4,3 -> in array: 1,0,3,2
        // highlight node for 

        int[] match_index = { 1,0,3,2 };

        ToolTip correctLeave = leaves[match_index[leave_index_in_tree]];

        correctLeave.transform.localPosition += new Vector3(0, 0, -0.05f);
        correctLeave.transform.localScale += new Vector3(0.5f, 0.5f, 0);


    }

    /// <summary>
	/// removes the highlight by scaling it back to normal 
	/// </summary>
	/// <param name="leave_index_in_tree"></param>
    public void RemoveHighlightLeave(int leave_index_in_tree)
    {
        int[] match_index = { 1, 0, 3, 2 };

        ToolTip correctLeave = leaves[match_index[leave_index_in_tree]];

        correctLeave.transform.localPosition -= new Vector3(0, 0, -0.05f);
        correctLeave.transform.localScale -= new Vector3(0.5f, 0.5f, 0);
    }



}
