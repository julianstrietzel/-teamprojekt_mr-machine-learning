using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rebuild_Layer:Layer
{

    public string info = "this is a rebuild layer";

    public Rebuild_Layer(int level, int expectedDPs, Layer previousLayer, Rebuild_DecisionTree decisionTreeHandler) : base(level, expectedDPs, previousLayer, decisionTreeHandler)   
    {
        
    }


    public void Deactivate()
    {
        ((Rebuild_DecisionTree)decisionTree).MoveDowntoRebuild();
        foreach(GameObject nodeGameObject in nodes)
        {
            nodeGameObject.GetComponent<Rebuild_FrameHandler>().DestroyThisPart();
        }
        nodes.Clear();
        ((Rebuild_Layer)prevLayer).Reactivate();

        DecisionTreeHandler.s_layers.Remove(NextLayer());
        foreach(Rebuild_Layer layer in DecisionTreeHandler.s_layers)
        {
            int i = DecisionTreeHandler.s_layers.Count;
            if (layer.layerLevel > this.layerLevel) DecisionTreeHandler.s_layers.Remove(layer);
            Debug.Assert((i == DecisionTreeHandler.s_layers.Count + 1 && layer.layerLevel > this.layerLevel) || !(layer.layerLevel > this.layerLevel) , "Next layer has not been removed from List" );
        }
        Debug.Log("deactivated");
    }

    public void Reactivate()
    {
        foreach (GameObject nodeGameObject in nodes)
        {
            nodeGameObject.GetComponent<FrameHandler>().Activate();
        }
        ReplaceButtonCalltoThis();
    }

    public override void Activate()
    {
        base.Activate();
        Debug.Log(decisionTree.move + "move in Activate afte base activation called");
        ReplaceButtonCalltoThis();
    }

    public void ReplaceButtonCalltoThis()
    {
        UnityAction deact = Deactivate;
        if (layerLevel == 0) deact = null;
        ((Rebuild_DecisionTree)decisionTree).ReplaceListenerToRebuildButton(deact);
    }

    public override Layer NextLayer()
    {
        if (DecisionTreeHandler.s_layers.Count <= layerLevel + 1)
        {
            DecisionTreeHandler.s_layers.Add(new Rebuild_Layer(layerLevel + 1, countDps - countFinallyFiltered, this, ((Rebuild_DecisionTree)decisionTree)));
        }
        return (Layer)DecisionTreeHandler.s_layers[layerLevel + 1];
    }





}
