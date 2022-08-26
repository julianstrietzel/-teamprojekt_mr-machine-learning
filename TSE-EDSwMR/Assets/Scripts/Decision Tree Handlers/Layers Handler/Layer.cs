using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
    protected ArrayList nodes = new ArrayList(); //List of nodes in the layer to be placed 
    public readonly int countDps; //number of datapoints in this layer to set frame size relative to this depending on the #dps in the node. Is already defined by the layer above
    protected int countFinallyFiltered; // number of datapoints, that are perfectly seperated in this layer. In combination we now know the number of datapoints for the next layer
    public readonly int layerLevel;
    protected Layer prevLayer;
    protected DecisionTreeHandler decisionTree;


    public  Layer(int level, int expectedDPs, Layer previousLayer, DecisionTreeHandler decisionTreeHandler)
    {
        decisionTree = decisionTreeHandler;
        layerLevel = level;
        countDps = expectedDPs;
        prevLayer = previousLayer;
    }

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

    public virtual Layer NextLayer()
    {
        if (DecisionTreeHandler.s_layers.Count <= layerLevel + 1)
        {
            DecisionTreeHandler.s_layers.Add(new Layer(layerLevel + 1, countDps - countFinallyFiltered, this, decisionTree));
        }
        return (Layer)DecisionTreeHandler.s_layers[layerLevel + 1];
    }


    //Returns the number of dps to the left of the given frame to place ´his childs accordingly
    //returns -1 if node is not in layer
    //Wird aufgerufen von einem Knoten in dieser Layer, welcher einen neuen Knoten darunter platzieren will
    public int GetCountDPsToTheLeftForNextLayer(FrameHandler frame)
    {

        int result = 0;
        int i = 0;
        if (!(nodes.Count > 0))
        {
            return -1;
        }
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



    public bool LayerIsReady()
    {
        foreach (GameObject node in nodes)
        {
            if (!node.GetComponent<FrameHandler>().IsReady()) return false;
        }
        return true;
    }

    public bool IsEmpty()
    {
        return nodes.Count == 0;
        //TODO do something if no node containes any 
    }


    public void ListenerNodeGeneratesChildren()
    {
        if (!LayerIsReady()) return;
        NextLayer().Activate();
    }

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
