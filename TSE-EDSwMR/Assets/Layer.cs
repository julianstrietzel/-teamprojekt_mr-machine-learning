using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
    ArrayList nodes = new ArrayList(); //List of nodes in the layer to be placed 
    public readonly int countDps; //number of datapoints in this layer to set frame size relative to this depending on the #dps in the node. Is already defined by the layer above
    int countFinallyFiltered = 0; // number of datapoints, that are perfectly seperated in this layer. In combination we now know the number of datapoints for the next layer
    public readonly int layerLevel;
    private Layer prevLayer;
    private DecisionTreeHandler decisionTree;


    public Layer(int level, int expectedDPs, Layer previousLayer, DecisionTreeHandler decisionTreeHandler)
    {
        decisionTree = decisionTreeHandler;
        layerLevel = level;
        countDps = expectedDPs;
        prevLayer = previousLayer;
    }

    public void AddNode(GameObject frame)
    {
        FrameHandler frameHandler = frame.GetComponent<FrameHandler>();

        countFinallyFiltered += frameHandler.Singular() ? frameHandler.NumberDatapoints() : 0 ;

        int newNumberForSort = frameHandler.numberForSorting;
        int i = 0;
//TODO for each instead of while 
        while (i < nodes.Count)
        {
            if(((GameObject)nodes[i]).GetComponent<FrameHandler>().numberForSorting > newNumberForSort)
            {
                nodes.Insert(i, frame);
                return;
            }
            i++;
           
        }
        nodes.Add(frame); //fallback if last object
    }

    public Layer NextLayer()
    {
        if(DecisionTreeHandler.s_layers.Count <= layerLevel + 1)
        {
            DecisionTreeHandler.s_layers.Add(new Layer(layerLevel + 1, countDps - countFinallyFiltered, this, decisionTree));
        }
        Layer newLayer = (Layer) DecisionTreeHandler.s_layers[layerLevel + 1];
        return newLayer;
    }


    //Returns the number of dps to the left of the given frame to place ´his childs accordingly
    //returns -1 if node is not in layer
    //Wird aufgerufen von einem Knoten in dieser Layer, welcher einen neuen Knoten darunter platzieren will
    public int GetCountDPsToTheLeftForNextLayer(FrameHandler frame)
    {

        int result = 0;
        int i = 0;
        if(!(nodes.Count > 0))
        {
            return -1;
        }
        FrameHandler it_frame = ((GameObject)nodes[i]).GetComponent<FrameHandler>();
        while (!it_frame.Equals(frame))
        {
            result += it_frame.Singular()? 0 : it_frame.NumberDatapoints();


            i++;
            if(!(i < nodes.Count))
            {
                return -1;
            }
            it_frame = ((GameObject)nodes[i]).GetComponent<FrameHandler>();
        }
        return result;
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
        //TODO make if no node containes any 
    }


    public void ListenerNodeGeneratesChildren()
    {
        if (!LayerIsReady()) return;
        if(NextLayer().IsEmpty()) 
        { 
            //TODO do something if everything is empty or ready
        }
        NextLayer().Activate();

    }

    public void Activate()
    {

        decisionTree.MoveUpForNextLayer();

        foreach (GameObject nodeGameObject in nodes)
        {
            nodeGameObject.GetComponent<FrameHandler>().Activate();
        }
    }



}
