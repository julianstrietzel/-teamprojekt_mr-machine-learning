using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Adds rebuild functionality to Layer
/// Used in Rebuild DecisionTrees
/// </summary>
public class Rebuild_Layer:Layer
{

    public string info = "this is a rebuild layer";

    //Forwards to base constructor
    public Rebuild_Layer(int level, int expectedDPs, Layer previousLayer, Rebuild_DecisionTree decisionTreeHandler) : base(level, expectedDPs, previousLayer, decisionTreeHandler)   
    {
        
    }

    /// <summary>
    /// Deactivates the layer:
    /// 1. Destroys all child parts of this layer
    /// 2. Moves down the tree
    /// 3. Reactivates prev. Layer
    /// 
    /// </summary>
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
    }
    /// <summary>
    /// Like Activate only does not move up the tree and does not check  for any singularities
    /// </summary>
    public void Reactivate()
    {
        foreach (GameObject nodeGameObject in nodes)
        {
            nodeGameObject.GetComponent<FrameHandler>().Activate();
        }
        ReplaceRebuildButtonCalltoThis();
        ((Rebuild_DecisionTree) decisionTree).Dissable_Continue_Button();
    }


    public void InitialActivation(DialogResult res)
    {
        Reactivate();
    }

    /// <summary>
    /// Adds the Replace Button Call to this Layer, so it will deactivate this layer on replace button clicked
    /// </summary>
    public override void Activate()
    {
        ReplaceRebuildButtonCalltoThis();
        base.Activate();
    }

    private void ReplaceRebuildButtonCalltoThis()
    {
        UnityAction deact = Deactivate;
        if (layerLevel == 0) deact = null;
        ((Rebuild_DecisionTree)decisionTree).ReplaceListenerToRebuildButton(deact);
    }

    /// <summary>
    /// Creates new REbuildLayers on nextlayer
    /// </summary>
    /// <returns></returns>
    public override Layer NextLayer()
    {
        if (DecisionTreeHandler.s_layers.Count <= layerLevel + 1)
        {
            DecisionTreeHandler.s_layers.Add(new Rebuild_Layer(layerLevel + 1, countDps - countFinallyFiltered, this, ((Rebuild_DecisionTree)decisionTree)));
        }
        return (Layer)DecisionTreeHandler.s_layers[layerLevel + 1];
    }





}
