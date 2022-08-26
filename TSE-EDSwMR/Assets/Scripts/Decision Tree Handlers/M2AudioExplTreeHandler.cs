using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    public override void Dissable_Following()
    {
        m2AudioHandler.PlayIntroduction();
        base.Dissable_Following();
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

    public override void NodeIsSingular()
    {
        if(firstSingularNode) StartCoroutine(PlaySingularCoroutine());
        firstSingularNode = false;
        base.NodeIsSingular();
    }

    private IEnumerator PlaySingularCoroutine()
    {
        yield return new WaitForSeconds(10);
        m2AudioHandler.PlaySingularNodeExplanation();
    }

    public override void Finished()
    {
        m2AudioHandler.PlaySumUp();
        base.Finished();
    }

    public override void ContinueButtonPressed()
    {
        base.ContinueButtonPressed();
        SceneManager.LoadScene("M3RebuildingTreeScene");
    }

    public override void Hint()
    {
        string message = "In this module you are trying to build a decision tree from the given data.\n" +
            "The Tennisballs on the table are used to represent the datapoints you collected. The frames are the nodes of the decision tree.They are color coded so you know the parent of each node. \n" +
            "Use the buttons to choose a category to sort the datapoints. Your goal is to have only yes or no days (yellow or red tennisballs) in each node.";
        Dialog.Open(hint_prefab, DialogButtonType.OK, "Hint", message, true);
    }


}
