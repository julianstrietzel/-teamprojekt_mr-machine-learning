using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScriptPOV : MonoBehaviour
{

    public static readonly int AMOUNT_ICONS = 3;
    public static readonly int AMOUNT_DAYS = 4;
    public static readonly int NR_ICONS_NEW_DAY = 0;

    public IconsPOV iconsHandler;

    public DataHandlerPOV dataHandler;
    public AudioHandlerScript audioHandler;
    public GameObject kai;
    public GameObject decisionTree;

    public GameObject textAndHints;

    [SerializeField] GameObject continueButton;

    [SerializeField] Vector3[] positionIcons = new Vector3[AMOUNT_ICONS];

    [SerializeField] Vector3[] scaleIcons = new Vector3[AMOUNT_ICONS];

    

    [SerializeField] GameObject[] iconsDay1Prefab = new GameObject[AMOUNT_ICONS];
    [SerializeField] GameObject[] iconsDay2Prefab = new GameObject[AMOUNT_ICONS];
    [SerializeField] GameObject[] iconsDay3Prefab = new GameObject[AMOUNT_ICONS];
    [SerializeField] GameObject[] iconsDay4Prefab = new GameObject[AMOUNT_ICONS];


    private RetrobotAnimations kaiAnimations;
    private GameObject decisionTreeFinished;


    private int day = 1;
    private int icon_nr = 0;
    private bool finishedGame = false;
    private bool intro = true;
    private int audio_nr = 0;


    // Parameter in Kai's animator. checking the parameter through the animator is power consuming
    private bool kai_animator_Is_Talking;


    private List<GameObject[]> iconsList = new List<GameObject[]>();



    // Start is called before the first frame update
    void Start()
    {
        decisionTree.SetActive(false);


        iconsList.Add(iconsDay1Prefab);
        iconsList.Add(iconsDay2Prefab);
        iconsList.Add(iconsDay3Prefab);
        iconsList.Add(iconsDay4Prefab);

        iconsHandler.SetIconPositionScale(iconsList, positionIcons, scaleIcons);
        kaiAnimations = kai.GetComponent<RetrobotAnimations>();
     
        kaiAnimations.InitiateController(); // otherwise NullPointer because Start is called after the first animation is needed 

        StartCoroutine(PlayAndTalkNextClipCoroutine()); // Intro
     
    }

    // Update is called once per frame
    void Update()
    {

        if(intro == true && !audioHandler.isPlaying())
        {
            // start the choosing phase
            intro = false;
            StartCoroutine(TalkAndShowNext());
        }


    }

    /// <summary>
    /// inform data handler about Yes decision on current day and show next day
    /// </summary>
    public void Yes_Clicked()
    {
        dataHandler.SetFinalDecision(day, true);

        StartCoroutine(TalkAndShowNext());

    }
    /// <summary>
    /// inform data handler about No decision on current day and show next day
    /// </summary>
    public void No_Clicked()
    {

        dataHandler.SetFinalDecision(day, false);
        StartCoroutine(TalkAndShowNext());
    }

    /// <summary>
    /// Start the previous audio again and kai's animation
    /// </summary>
    public void RestartAudio()
    {
        StartCoroutine(PlayAndTalkCoroutine(audio_nr - 1));
    }

    /// <summary>
    /// Call this to open the Hint
    /// </summary>
    public void Hint()
    {
        // TODO pause audiosource

        if (!finishedGame)
        {
            textAndHints.GetComponent<TextAndHintsPOV>().HintChoosingPhase();
        }
        else
        {
            textAndHints.GetComponent<TextAndHintsPOV>().HintExplanationPhase();
        }
    }

    private void PauseAudioAndKai()
    {
        // TODO audio handler stop
        kaiAnimations.StopTalking();
    }

    private void OpenInformationPanel()
    {
        string message = "- Root" + "\n\n" + "- Inner Node" + "\n\n" + "- Leave" + "\n\n" + "- Attribute" + "\n\n";
        message += "\n" + "Use: automization of complex, but systematic decisions.";
        message += "\n\n" + "Open the Hint to read the explanantion.";
        textAndHints.GetComponent<TextAndHintsPOV>().informationPanelPrefab.GetComponent<SolverHandler>().AdditionalOffset = new Vector3(-0.3f, -0.3f, 0);
        Dialog.Open(textAndHints.GetComponent<TextAndHintsPOV>().informationPanelPrefab, DialogButtonType.None, "Terminology     ", message, false); ;

    }

    /// <summary>
    /// In the choosing phase of the module shows the next icon and plays the audio
    /// </summary>
    /// <returns></returns>
    private IEnumerator TalkAndShowNext()
    {
        int iconNrWaitForInput = 1;
        while(iconNrWaitForInput <= AMOUNT_ICONS && !finishedGame)
        {
            float clip_length = audioHandler.DurationAudio(audio_nr);
            ShowNext();
            StartCoroutine(PlayAndTalkNextClipCoroutine());
            yield return new WaitForSeconds(clip_length);
            iconNrWaitForInput++;
        }
    }


    /// <summary>
    /// Whole explanation of decision tree
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayExplanationDecisionTree()
    {


        POV_DecisionTree dt = decisionTree.GetComponent<POV_DecisionTree>();
        //intro
        StartCoroutine(PlayAndTalkCoroutine(13)); 
        yield return new WaitForSeconds(audioHandler.DurationAudio(13) + 1);

        // example datapoint, show icons of datapoint, explain the example
        dt.InstantiateExampleDatapoint();
        StartCoroutine(PlayAndTalkCoroutine(14));
        yield return new WaitForSeconds(3);

        //highlight the nodes in tree
        StartCoroutine(HighlightTreeNodesForExampleDatapoint(dt));     
        yield return new WaitForSeconds(audioHandler.DurationAudio(14) - 2);
        //  remove example object
        dt.DestroyExample();

        // decision tree explanation name
        StartCoroutine(PlayAndTalkCoroutine(15));
        yield return new WaitForSeconds(audioHandler.DurationAudio(15));

       
        // explain root
        StartCoroutine(PlayAndTalkCoroutine(16));
        dt.HighlightRoot();
        yield return new WaitForSeconds(audioHandler.DurationAudio(16));
        dt.RemoveHighlightRoot();


        // show inner node
        StartCoroutine(PlayAndTalkCoroutine(17));
        dt.HighlightInnerNode();
        yield return new WaitForSeconds(audioHandler.DurationAudio(17) + 0.5f);
        dt.RemoveHighlightInnerNode();


        // leaves
        StartCoroutine(PlayAndTalkCoroutine(18));

        dt.HighlightLeave(0);
        yield return new WaitForSeconds(audioHandler.DurationAudio(18));
        dt.RemoveHighlightLeave(0);

        // attribute 

        StartCoroutine(PlayAndTalkCoroutine(19));
        yield return new WaitForSeconds(audioHandler.DurationAudio(19) + 0.5f);


        // move Kai back to see panel better
        kai.transform.localPosition += new Vector3(0, 0, 0.3f);
        // open information panel
        OpenInformationPanel();

        // enough theory
        StartCoroutine(PlayAndTalkCoroutine(20));
        yield return new WaitForSeconds(audioHandler.DurationAudio(20) + 0.5f);

        // use 

        StartCoroutine(PlayAndTalkCoroutine(21));
        yield return new WaitForSeconds(audioHandler.DurationAudio(21) + 0.5f);


        // next module

        StartCoroutine(PlayAndTalkCoroutine(22));
        yield return new WaitForSeconds(audioHandler.DurationAudio(22) + 0.5f);


        continueButton.SetActive(true);

    
    }



    /// <summary>
    /// with given decision tree script highlight the tree nodes for the example data point
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private IEnumerator HighlightTreeNodesForExampleDatapoint(POV_DecisionTree dt)
    {
        // highlight nodes in tree
        foreach (GameObject node in dt.nodesExample)
        {
            dt.HighlightNode(node);
            yield return new WaitForSeconds(2);
            dt.RemoveHighlightNode(node);
        }

        // highlight leave 

        dt.HighlightLeave(dt.indexInTreeExampleLeave);
        yield return new WaitForSeconds(2);
        dt.RemoveHighlightLeave(dt.indexInTreeExampleLeave);
    }

    /// <summary>
    /// plays next audio with animation, for the last icon retrobot presents left
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayAndTalkNextClipCoroutine()
    {
        audioHandler.PlayAudioClipNr(audio_nr);
        float clip_length = audioHandler.DurationAudio(audio_nr);
        audio_nr++;
        if (icon_nr == AMOUNT_ICONS)
        {
            yield return kaiAnimations.TalkAndPresentLeftCoroutine(clip_length);
        }
        else
        {
            yield return kaiAnimations.TalkForCoroutine(clip_length);
        }

    }

    private IEnumerator PlayAndTalkCoroutine(int clip_nr)
    {
        audioHandler.PlayAudioClipNr(clip_nr);
        yield return kaiAnimations.TalkForCoroutine((float)audioHandler.DurationAudio(clip_nr));
    }

    /// <summary>
    /// show next icon or show decision tree if the game finished
    /// </summary>
    private void ShowNext()
    {
        UpdateCurrentState();

        if (finishedGame && iconsHandler != null)
        {
            iconsHandler.DestroyParentAfterGameFinish();
            ShowDecisionTree();
            StartCoroutine(PlayExplanationDecisionTree());

        }
        else
        {
            iconsHandler.DisplayNextIcon(day, icon_nr);
        }
    }

    private void ShowDecisionTree()
    {
        decisionTree.GetComponent<POV_DecisionTree>().InitiateTree(dataHandler);
        decisionTree.SetActive(true);

    }

    /// <summary>
    /// update the state which day and icon it is 
    /// </summary>
    private void UpdateCurrentState()
    {
        if (icon_nr >= AMOUNT_ICONS)
        {
            icon_nr = 0;
            day++;
        }

        if (day > AMOUNT_DAYS)
        {
            finishedGame = true;
        }

        else
        {
            icon_nr++;
        }

    }

    public bool GetFinishedGame()
    {
        return finishedGame;
    }

    public bool GetIntroIsPlaying()
    {
        return intro;
    }


}
