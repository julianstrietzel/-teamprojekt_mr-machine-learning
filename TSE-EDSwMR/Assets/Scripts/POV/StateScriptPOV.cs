using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateScriptPOV : MonoBehaviour
{
    //TODO add hand menu to POV Scene

    public static readonly int AMOUNT_ICONS = 3;
    public static readonly int AMOUNT_DAYS = 4;
    public static readonly int NR_ICONS_NEW_DAY = 0;

    public IconsPOV iconsHandler;

    public DataHandlerPOV dataHandler;
    public AudioHandlerScript audioHandler;
    public GameObject kai;
    public GameObject decisionTree;


    [SerializeField] Vector3[] positionIcons = new Vector3[AMOUNT_ICONS];

    [SerializeField] Vector3[] scaleIcons = new Vector3[AMOUNT_ICONS];



    [SerializeField] GameObject[] iconsDay1Prefab = new GameObject[AMOUNT_ICONS];
    [SerializeField] GameObject[] iconsDay2Prefab = new GameObject[AMOUNT_ICONS];
    [SerializeField] GameObject[] iconsDay3Prefab = new GameObject[AMOUNT_ICONS];
    [SerializeField] GameObject[] iconsDay4Prefab = new GameObject[AMOUNT_ICONS];

    //private RetrobotAnimations kai;

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

    //private static int AMOUNT_ICONS = 3;

    //// IDEA: putting the icons in a list would be more flexible. But harder to see the order, additional its also harder to say which position it should go to
    //// what if the position is also set in unity?


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

        //if (finishedGame)
        //{
        //    StartCoroutine(PlayExplanationDecisionTree()); //TODO seems to repeat

        //}
        //Debug.Log("TalkAndShowNext Explantions decsion tree ");



    }
    //private IEnumerator TalkAndShowNext()
    //{

    //    float clip_length = audioHandler.DurationAudio(audio_nr);
    //    ShowNext();
    //    StartCoroutine(PlayAndTalkNextClipCoroutine());
    //    yield return new WaitForSeconds(clip_length);

    //    clip_length = audioHandler.DurationAudio(audio_nr);

    //    ShowNext();
    //    StartCoroutine(PlayAndTalkNextClipCoroutine());
    //    yield return new WaitForSeconds(clip_length);

    //    clip_length = audioHandler.DurationAudio(audio_nr);

    //    ShowNext();
    //    StartCoroutine(PlayAndTalkNextClipCoroutine());
    //    yield return new WaitForSeconds(clip_length);

    //}











    private IEnumerator PlayExplanationDecisionTree()
    {
        //TODO pauses between explanations

        Debug.Log("Intro");
        POV_DecisionTree dt = decisionTree.GetComponent<POV_DecisionTree>();
        //intro
        StartCoroutine(PlayAndTalkCoroutine(13)); 
        yield return new WaitForSeconds(audioHandler.DurationAudio(13) + 1);

        Debug.Log("Exmaple");

        // example datapoint, show icons of datapoint, explain the example
        dt.InstantiateExampleDatapoint();
        StartCoroutine(PlayAndTalkCoroutine(14));
        yield return new WaitForSeconds(3);

        //highlight the nodes in tree
        StartCoroutine(HighlightTreeNodesForExampleDatapoint(dt));     
        yield return new WaitForSeconds(audioHandler.DurationAudio(14) - 2);
        // TODO remove example object
        dt.DestroyExample();
        Debug.Log("upside down tree");

        // decision tree explanation name
        StartCoroutine(PlayAndTalkCoroutine(15));
        yield return new WaitForSeconds(audioHandler.DurationAudio(15));

        Debug.Log("Root");

        // explain root
        StartCoroutine(PlayAndTalkCoroutine(16));
        dt.HighlightRoot();
        yield return new WaitForSeconds(audioHandler.DurationAudio(16));
        dt.RemoveHighlightRoot();

        Debug.Log("inner node");

        // show inner node
        StartCoroutine(PlayAndTalkCoroutine(17));
        dt.HighlightInnerNode();
        yield return new WaitForSeconds(audioHandler.DurationAudio(17));
        dt.RemoveHighlightInnerNode();

        Debug.Log("leaves");

        // leaves
       

        for (int nr = 18; nr < 21; nr++)
        {
            StartCoroutine(PlayAndTalkCoroutine(nr));
            yield return new WaitForSeconds(audioHandler.DurationAudio(nr) + 0.5f);
        }
   

        //TODO add buttons for next menu
        // TODO add hint to go to the next menu?
        // use update for all the different phases?



    }

    /// <summary>
    /// with given decision tree script highlight the tree nodes for the example data point
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private IEnumerator HighlightTreeNodesForExampleDatapoint(POV_DecisionTree dt)
    {
        foreach (GameObject node in dt.nodesExample)
        {
            Debug.Log("State script highlight for example node: " + node);
            dt.HighlightNode(node);
            yield return new WaitForSeconds(2);
            dt.RemoveHighlightNode(node);
        }

    }

    /// <summary>
    /// plays next audio with animation, for the last icon retrobot presents left
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayAndTalkNextClipCoroutine()
    {
        Debug.Log("Next clip Corutine; clip: " + audio_nr);

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

    IEnumerator PlayAndTalkCoroutine(int clip_nr)
    {
        Debug.Log("State; Play and Talk Corutine");
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
            StartCoroutine(PlayExplanationDecisionTree()); //TODO wrong place 

        }
        else
        {
            iconsHandler.DisplayNextIcon(day, icon_nr);
        }
    }

    private void ShowDecisionTree()
    {
        Debug.Log("Show decision tree");

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
            Debug.Log(" set finishedGame ==: " + finishedGame);

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
