using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.UI;

public class Rebuild_DecisionTree : DecisionTreeHandler
{

    bool movedown = false;
    public GameObject rebuild_prefab;
    private GameObject rebuild_button;

    public override void Update()
    {
        base.Update();
        if (movedown)
        {
            moved += Vector3.Distance(Vector3.forward * Time.deltaTime * speed, Vector3.zero);

            transform.Translate(-1 * Vector3.forward * Time.deltaTime * speed);
            if (moved > .3f * (1f + buffer))
            {
                movedown = false;
                moved = 0f;
            }
        }

    }


    public override void OnDataHandlerInit()
    {
        GameObject root = Instantiate(frame_prefab, gameObject.transform);
        Rebuild_Layer layerZero = new Rebuild_Layer(0, DataHandler.data.Count, null, this);
        FrameHandler roothandler = root.GetComponent<FrameHandler>();

        s_layers = new ArrayList();
        s_layers.Add(layerZero);
        prev_color = Color.blue;
        roothandler.InitFrame(new List<string>(), DataHandler.data, layerZero, 1, 0, prev_color);

        place_button = Instantiate(button_prefab, gameObject.transform.parent.parent);
        place_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Placed correctly?";

        rebuild_button = Instantiate(rebuild_prefab, gameObject.transform.parent.transform);
        rebuild_button.SetActive(false);

        UnityEvent button_pressed = EnableFollowing();
        button_pressed.AddListener(Dissable_Following);
        button_pressed.AddListener(roothandler.Activate);
        button_pressed.AddListener(DeactivateTooltip);
        button_pressed.AddListener(InitRebuildButton);
        



    }
    //TODO add button to delete filter for every node and 

    private void InitRebuildButton()
    {
        rebuild_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Rebuild Layer";
    }


    public void MoveDowntoRebuild()
    {
        movedown = true;
    }

    

    public void ReplaceListenerToRebuildButton(UnityAction call)
    {
        if (call == null) {
            rebuild_button.SetActive(false);
            return;
        }
        rebuild_button.SetActive(true);
        rebuild_button.GetComponent<PressableButtonHoloLens2>().ButtonPressed.RemoveAllListeners();
        rebuild_button.GetComponent<PressableButtonHoloLens2>().ButtonPressed.AddListener(call);
    }

    
}
