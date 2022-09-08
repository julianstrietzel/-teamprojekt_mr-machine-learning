using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DecisionTrees are built from layers, which contain all the nodes
/// They sum up important information for data capsuling and how many nodes are singular/empty 
/// </summary>
public class Layer
{
    protected ArrayList nodes = new ArrayList(); //List of nodes in the layer to be placed 
    public readonly int countDps; //number of datapoints in this layer to set frame size relative to this depending on the #dps in the node. Is already defined by the layer above
    protected int countFinallyFiltered; // number of datapoints, that are perfectly seperated in this layer. In combination we now know the number of datapoints for the next layer
    public readonly int layerLevel;
    protected Layer prevLayer;
    protected DecisionTreeHandler decisionTree;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="level">layer of the node starting from zero</param> 
    /// <param name="expectedDPs">all dps in prev layer minus singular node's dps</param>
    /// <param name="previousLayer">null on first layer</param>
    /// <param name="decisionTreeHandler">respective DT Handler</param>
    public Layer(int level, int expectedDPs, Layer previousLayer, DecisionTreeHandler decisionTreeHandler)
    {
        decisionTree = decisionTreeHandler;
        layerLevel = level;
        countDps = expectedDPs;
        prevLayer = previousLayer;
    }

    /// <summary>
    /// Adds a node to this layer.
    /// Updates statistics with this node
    /// Adds this node to datastructured
    /// </summary>
    /// <param name="newframe">new Frame Gameobject, needs to contain FrameHandler Component</param>
    public void AddNode(GameObject newframe)
    {
        FrameHandler newFrameHandler = newframe.GetComponent<FrameHandler>();

        countFinallyFiltered += newFrameHandler.Singular() ? newFrameHandler.NumberDatapoints() : 0;

        if (newFrameHandler.Singular()) decisionTree.NodeIsSingular(newFrameHandler);

        int newNumberForSort = newFrameHandler.numberForSorting;
        int i = 0;

        while (i < nodes.Count)
        {
            if (GetFrameHandler(i).numberForSorting > newNumberForSort)
            {
                nodes.Insert(i, newframe);
                return;
            }
            i++;

        }
        nodes.Add(newframe); //fallback if last object
    }

    /// <summary>
    /// Returns new Layer.
    /// Creates new Layer if none is there
    /// </summary>
    /// <returns></returns>
    public virtual Layer NextLayer()
    {
        if (DecisionTreeHandler.s_layers.Count <= layerLevel + 1)
        {
            DecisionTreeHandler.s_layers.Add(new Layer(layerLevel + 1, countDps - countFinallyFiltered, this, decisionTree));
        }
        return (Layer)DecisionTreeHandler.s_layers[layerLevel + 1];
    }

    /// <summary>
    /// Returns the number of dps to the left without singularsof the given frame to place his childs accordingly
    /// Called from Node in this layer which wants to position a new node in next layer
    /// </summary>
    /// <param name="frame"></param>
    /// <returns>number of dps to left, returns -1 if node is not in layer</returns>
    public int GetCountDPsToTheLeftForNextLayer(FrameHandler frame)
    {
        int result = 0;
        int i = 0;
        if (!(nodes.Count > 0)) return -1;
        
        FrameHandler it_frame = ((GameObject)nodes[i]).GetComponent<FrameHandler>();
        while (!it_frame.Equals(frame))
        {
            result += it_frame.Singular() ? 0 : (int)System.Math.Ceiling(it_frame.NumberDatapoints() / 2f) * 2;
            i++;
            if (!(i < nodes.Count))
            {
                return -1;
            }
            it_frame = GetFrameHandler(i);
        }
        return result;
    }

    
    protected FrameHandler GetFrameHandler(int i)
    {
        return ((GameObject)nodes[i]).GetComponent<FrameHandler>();
    }


    /// <summary>
    /// Checks for all nodes being ready
    /// </summary>
    /// <returns>All nodes ready??</returns>
    public bool LayerIsReady()
    {
        foreach (GameObject node in nodes)
        {
            if (!node.GetComponent<FrameHandler>().IsReady()) return false;
        }
        return true;
    }

    /// <summary>
    /// No nodes in this class
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        return nodes.Count == 0;
    }

    /// <summary>
    /// ACtivates next layer if this is ready.
    /// Called in Node if the it generates children, to check if this is the last node to do so
    /// </summary>
    public void ListenerNodeGeneratesChildren()
    {
        if (!LayerIsReady()) return;
        NextLayer().Activate();
    }

    /// <summary>
    /// Activates all nodes in this layer.
    /// If they are all singular calls Finished()
    /// </summary>
    public virtual void Activate()
    {
        bool atleastonenodenotsingular = false;
        decisionTree.MoveUpForNextLayer();

        foreach (GameObject nodeGameObject in nodes)
        {
            FrameHandler hand = nodeGameObject.GetComponent<FrameHandler>();
            hand.Activate();
            atleastonenodenotsingular = !hand.Singular() || atleastonenodenotsingular;
        }
        if (!atleastonenodenotsingular) decisionTree.Finished();
    }



}
