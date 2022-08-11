using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rebuild_Layer:Layer
{

   

    public Rebuild_Layer(int level, int expectedDPs, Layer previousLayer, Rebuild_DecisionTree decisionTreeHandler) : base(level, expectedDPs, previousLayer, decisionTreeHandler)   
    {
    }


    public void Deactivate()
    {
        ((Rebuild_DecisionTree)decisionTree).MoveDowntoRebuild();
        foreach(GameObject nodeGameObject in nodes)
        {
            nodeGameObject.SetActive(false);
        }
        nodes.Clear();
        ((Rebuild_Layer)prevLayer).Reactivate();
        DecisionTreeHandler.s_layers.Remove(NextLayer());
    }

    public void Reactivate()
    {
        foreach (GameObject nodeGameObject in nodes)
        {
            nodeGameObject.GetComponent<FrameHandler>().Activate();
        }
        SetResetButton();
    }

    public override void Activate()
    {
        base.Activate();
        SetResetButton();
    }

    public void ResetButtonPressed()
    {
        
    }

    public void SetResetButton()
    {
        //TODO 1 UnityEvent event = ((Rebuild_DecisionTree)decisionTree).ResetButtonEvent();
    }





}
