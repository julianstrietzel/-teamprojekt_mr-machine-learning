using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Rebuild_DecisionTree : DecisionTreeHandler
{

    bool movedown = false;
    public GameObject rebuild_prefab;
    public GameObject small_dialog_prefab;
    private GameObject rebuild_button;
    bool isEntropyTree = false;
    private M3AudioHandler m3AudioHandler;
    private M4AudioHandler m4AudioHandler;
    private bool firstlayer = true;



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
        isEntropyTree = roothandler.isEntropyFrame;


        if (!isEntropyTree)
        {
            m3AudioHandler = Explaning.GetComponent<M3AudioHandler>();
            m3AudioHandler.PlayIntro();
        } else
        {
            m4AudioHandler = Explaning.GetComponent<M4AudioHandler>();
            //TODO explain information gain, entropy and the idea of ID3
        }

        place_button = Instantiate(button_prefab, gameObject.transform.parent.parent);
        place_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Placed correctly?";

        rebuild_button = Instantiate(rebuild_prefab, gameObject.transform.parent.transform);
        rebuild_button.transform.localScale *=  1f / transform.parent.localScale.x;
        rebuild_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Rebuild Layer";
        rebuild_button.SetActive(false);


        place_button_pressed = EnableFollowing();
        place_button_pressed.AddListener(Dissable_Following);
        place_button_pressed.AddListener(roothandler.Activate);
        place_button_pressed.AddListener(DeactivateTooltip);
    }



    public void MoveDowntoRebuild()
    {
        movedown = true;
    }

    public override void MoveUpForNextLayer()
    {
        base.MoveUpForNextLayer();
        if (firstlayer && !isEntropyTree)
        {
            m3AudioHandler.PlayAdditionalNotes();
            firstlayer = false;
        }
    }



    public void ReplaceListenerToRebuildButton(UnityAction call)
    {
        if (call == null)
        {
            rebuild_button.SetActive(false);
            return;
        }
        rebuild_button.SetActive(true);
        rebuild_button.GetComponent<PressableButtonHoloLens2>().ButtonPressed.RemoveAllListeners();
        rebuild_button.GetComponent<PressableButtonHoloLens2>().ButtonPressed.AddListener(call);
    }

    public override void Hint()
    {

        string message = "Now you also have the possibility to rebuild parts of the tree by pressing the \"rebuild\" button.\n" +
            "Your goal is to build a tree as flat as possible, so your algorithm runs optimized. Try different combinations by rebuilding single layers.\n" +
            "The idea is to choose the attributes first, that are most relevant for Kai's decision\n";
        
        if(isEntropyTree)
        {
            message = "In this Module we added Entropy and Information Gain as information. It measures which criteria are most relevant for the decision. This helps you to build the perfect tree.\n";
            message += "You still have the possibility to rebuild parts of the tree by pressing the \"rebuild\" button.\n" +
                "Your goal is to build a tree as flat as possible, so your algorithm runs optimized. \n \n";
            
        }
        message += "Previous Hint: \nIn this module you are trying to build a decision tree from the given data.\n" +
                 "The Tennisballs on the table are used to represent the datapoints you collected. The frames are the nodes of the decision tree.They are color coded so you know the parent of each node. \n" +
                 "Use the buttons to choose a category to sort the datapoints. Your goal is to have only yes or no days (yellow or red tennisballs) in each node.";

        Dialog.Open(hint_prefab, DialogButtonType.OK, "Hint", message, true);
    }

    public override void ContinueButtonPressed()
    {
        if(isEntropyTree)
        {
            Dialog.Open(small_dialog_prefab, DialogButtonType.OK, "Finished", "Thank you for taking this course. We hope you liked it.\n Use the hand menu to get back to the main menu.", true);
        } else
        {
            SceneManager.LoadScene("M4EntropyScene");
        }


        base.ContinueButtonPressed();
    }

    public void Dissable_Continue_Button()
    {
        if (continue_button == null) return;
        Destroy(continue_button);
    }

    public override void Finished()
    {
        if(!isEntropyTree && continue_button == null) m3AudioHandler.PlaySumUp();
        base.Finished();
    }




}
