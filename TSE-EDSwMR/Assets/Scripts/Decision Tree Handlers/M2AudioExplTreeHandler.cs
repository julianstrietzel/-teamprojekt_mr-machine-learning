using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Child of DecisionTreeHandler 
/// Adding functionality to play sounds and explenations in M2 at the correct positions
/// </summary>
public class M2AudioExplTreeHandler : DecisionTreeHandler
{
    private M2AudioHandler m2AudioHandler;
    private bool firstlayer = true;
    private bool firstSingularNode = true;

    public override void OnDataHandlerInit()
    {
        m2AudioHandler = Explaning.GetComponent<M2AudioHandler>();

        base.OnDataHandlerInit();
        place_button_pressed.AddListener(m2AudioHandler.PlayRootExplanation);
        m2AudioHandler.PlayIntroduction();

    }

    public override void MoveUpForNextLayer()
    {
        base.MoveUpForNextLayer();
        if (firstlayer)
        {
            m2AudioHandler.PlayFirstLayerExplanation();
            firstlayer = false;
        }
    }

    public override void NodeIsSingular(FrameHandler frame)
    {
        if(firstSingularNode) StartCoroutine(PlaySingularCoroutine(frame));
        firstSingularNode = false;
        base.NodeIsSingular(frame);

    }

    private IEnumerator PlaySingularCoroutine(FrameHandler frame)
    {
        if (s_layers.Count < 3) yield return new WaitForSeconds(40);
        m2AudioHandler.PlaySingularNodeExplanation();
    }

    public override void Finished()
    {
        StartCoroutine(EndCoroutine());
        
    }

    private IEnumerator EndCoroutine()
    {
        yield return new WaitForSeconds(10);
        m2AudioHandler.PlaySumUp();
        base.Finished();
    } 

    public override void ContinueButtonPressed()
    {
        base.ContinueButtonPressed();
        SceneManager.LoadScene("M3RebuildingTreeScene");
    }

    /// <summary>
    /// Hint specific for the M2
    /// </summary>
    public override void Hint()
    {
        string message = "In this module you are trying to build a decision tree from the given data.\n" +
            "The Tennisballs on the table are used to represent the datapoints you collected. The frames are the nodes of the decision tree.They are color coded so you know the parent of each node. \n" +
            "Use the buttons to choose a category to sort the datapoints. Your goal is to have only yes or no days (yellow or red tennisballs) in each node.";
        Dialog.Open(hint_prefab, DialogButtonType.OK, "Hint", message, true);
    }

    


}
