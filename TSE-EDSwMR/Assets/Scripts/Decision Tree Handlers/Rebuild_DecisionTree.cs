using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


/// <summary>
/// specific DecisionTreeHandler that enables the rebuilding of layers.
/// used in M3 and 4 depending on this audio is played
/// TODO extract parts that are only relevant for M4 or M3 and not necessary for the rebuilding part
/// </summary>
public class Rebuild_DecisionTree : DecisionTreeHandler
{

    [HideInInspector] bool movedown = false;
    public GameObject rebuild_prefab;
    public GameObject small_dialog_prefab;
    public GameObject large_dialog_prefab;
    public GameObject IG_Dialog_Prefab;
    public GameObject Entropy_Dialog_Prefab;
    private GameObject rebuild_button;
    bool isEntropyTree = false;

    private M3AudioHandler m3AudioHandler;
    private M4AudioHandler m4AudioHandler;
    private bool firstlayer = true;
    private bool m3SumUpPlayed = false;


    /// <summary>
    /// Adds movedown to update functionality
    /// </summary>
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

    /// <summary>
    /// Init changed only that correct types are used.
    /// Also adds rebuild button
    /// </summary>
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

        place_button = Instantiate(button_prefab, gameObject.transform.parent.parent);
        place_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Placed correctly?";

        rebuild_button = Instantiate(rebuild_prefab, gameObject.transform.parent.transform);
        rebuild_button.transform.localScale *=  1f / transform.parent.localScale.x;
        rebuild_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Rebuild Layer";
        rebuild_button.SetActive(false);


        place_button_pressed = EnableFollowing();
        place_button_pressed.AddListener(Dissable_Following);
        place_button_pressed.AddListener(DeactivateTooltip);

        //This could be extracted to child classes M3 and M4
        if (!isEntropyTree)
        {
            m3AudioHandler = Explaning.GetComponent<M3AudioHandler>();
            place_button_pressed.AddListener(PlayIntroM3);
            place_button_pressed.AddListener(roothandler.Activate);
        }
        else
        {
            m4AudioHandler = Explaning.GetComponent<M4AudioHandler>();
            place_button_pressed.AddListener(ExplainEntropy);
        }
    }



    /// <summary>
    /// Sets movedown to true so update will move tree back again
    /// </summary>
    public void MoveDowntoRebuild()
    {
        movedown = true;
    }

    /// Only adds audio from M3 to base func, which is called if this is the first movement
    /// <summary>
    /// </summary>
    public override void MoveUpForNextLayer()
    {
        base.MoveUpForNextLayer();
        if (firstlayer && !isEntropyTree)
        {
            m3AudioHandler.PlayAdditionalNotes();
            firstlayer = false;
        }
    }


    /// <summary>
    /// Sets function to call on Rebuild Button pressed
    /// If there is no function to call, this will deactivate the buttonn
    /// If the button is deactivated this will activate the button
    /// </summary>
    /// <param name="call"></param>
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

    /// <summary>
    /// Creates a hint dialog on Handmenu hint button pressed
    /// </summary>
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

    /// <summary>
    /// Goes to Menu
    /// Function to add to dialog.OnClose()
    /// Specific for M4
    /// </summary>
    /// <param name="res"></param>
    private void LoadMenu(DialogResult res)
    {
        SceneManager.LoadScene("Menu");
    } 

    /// <summary>
    /// Loads next scene or finished dialog with back to menu on close when continue button is pressed
    /// </summary>
    public override void ContinueButtonPressed()
    {
        if(isEntropyTree)
        {
            Dialog.Open(small_dialog_prefab, DialogButtonType.OK, "Finished", "Thank you for taking this course. We hope you liked it.\n Press okay to get back to the main menu.", true).OnClosed += LoadMenu;
        } else SceneManager.LoadScene("M4EntropyScene");
        base.ContinueButtonPressed();
    }

    /// <summary>
    /// Dissables the continue Button.
    /// Called if a layer is reactivated.
    /// </summary>
    public void Dissable_Continue_Button()
    {
        if (continue_button != null) Destroy(continue_button);        
    }

    /// <summary>
    /// Called if in the current layer all nodes are singular.
    /// Starts end coroutine for M3 and M4
    /// </summary>
    public override void Finished()
    {
        if (!isEntropyTree && continue_button == null)
        {
            if(!m3SumUpPlayed)
            {
                StartCoroutine(M3EndCoroutine());
            }        
        } else
        {
            if(s_layers.Count <= 3 || ((Layer)s_layers[3]).IsEmpty()) StartCoroutine(M4EndCoroutine());
            else Dialog.Open(small_dialog_prefab, DialogButtonType.OK, "Not the perfect DT", "Hey, that is not the perfect tree. \nDid you always click the category with the highest information gain?\nPlease try again, by rebuilding some layers.", true );
            return;
        }

        
    }


    private IEnumerator M3EndCoroutine()
    {
        yield return new WaitForSeconds(10);
        m3AudioHandler.PlaySumUp();
        m3SumUpPlayed = true;
        base.Finished();
    }
    private IEnumerator M4EndCoroutine()
    {
        rebuild_button.SetActive(false);
        yield return new WaitForSeconds(10);
        ExplainID3();
    }

    private void ExplainEntropy()
    {
        string message = "Entropy measures the randomness of a dataset. The higher the entropy, the more uncertainty is in it. \nLet S be the Dataset and P_yes is the proportion of Yes-Datapoints in it.\nIn our context this is the proportion of Days we go play tennis.";
        m4AudioHandler.ExplainEntropy();
        Dialog dialog = Dialog.Open(Entropy_Dialog_Prefab, DialogButtonType.OK, "Entropy", message, true);
        dialog.OnClosed += ExplainIG;
    }

    public void ExplainIG(DialogResult result)
    {
        string message = "Information Gain measures how much the average Entropy over the next subsets or nodes improves, by each category.\n" +
            "In the follwing always choose the category with the highest Information Gain.";
        m4AudioHandler.ExplainIG();
        Dialog dialog = Dialog.Open(IG_Dialog_Prefab, DialogButtonType.OK, "Information Gain", message, true);
        dialog.OnClosed += ((Rebuild_Layer) s_layers[0]).InitialActivation;
    }

    private void ExplainID3()
    {
        m4AudioHandler.ExplainID3();
        string message = "1. You calculate the Entropy and Information Gain for each category." +
            "\n2. Choose the category for the separation by the greatest Information Gain." +
            "\nRepeat step 1 and 2 till every node has entropy zero.";
        Dialog.Open(large_dialog_prefab, DialogButtonType.OK, "Algorithm ID3", message, true).OnClosed += M4ReallyFinished;
        
    }

    public void M4ReallyFinished(DialogResult res)
    {
        base.Finished();
    }

    public void PlayIntroM3()
    {
        m3AudioHandler.PlayIntro();
    }






}
