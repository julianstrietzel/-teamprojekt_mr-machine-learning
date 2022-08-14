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

        Debug.Log("in State Script, play intro");

        StartCoroutine(PlayAndTalkNextClipCoroutine()); // Intro
     
    }

    // Update is called once per frame
    void Update()
    {

        if(intro == true && !audioHandler.isPlaying())
        {

            Debug.Log("intro false");
            intro = false;


            StartCoroutine(ClickOnlyDaysCoroutine());
        }


    }

    void UpdateCurrentState()
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
   
    private void Clicked()
    {
        UpdateCurrentState();

        if (finishedGame && iconsHandler != null)
        {
            Debug.Log("Clicked if finished game");
            iconsHandler.DestroyParentAfterGameFinish();
            ShowDecisionTree();
        }
        else
        {
            iconsHandler.DisplayNextIcon(day, icon_nr);

        }

      
      

    }




    IEnumerator ClickOnlyDaysCoroutine()
    {

        Debug.Log("ClickOnlyDaysCoroutine");

        float clip_length = audioHandler.DurationAudio(audio_nr);
        Clicked();
        StartCoroutine(PlayAndTalkNextClipCoroutine());
        //Debug.Log("Start delay " + Time.time+ " clip length: "+ clip_length);
        yield return new WaitForSeconds(clip_length);

        clip_length = audioHandler.DurationAudio(audio_nr);

        //Debug.Log("second clicked at " + Time.time + "; new clip length: " +clip_length);
        Clicked();
        StartCoroutine(PlayAndTalkNextClipCoroutine());
        yield return new WaitForSeconds(clip_length);

        clip_length = audioHandler.DurationAudio(audio_nr);

        Clicked();
        StartCoroutine(PlayAndTalkNextClipCoroutine());
        yield return new WaitForSeconds(clip_length);

    }




    IEnumerator PlayAndTalkCoroutine(int clip_nr)
    {

        Debug.Log("Play and Talk Corutine");

        audioHandler.PlayAudioClipNr(clip_nr);

        //StartCoroutine(kaiAnimations.TalkForCoroutine((float)audioHandler.DurationAudio(0)));
        yield return kaiAnimations.TalkForCoroutine((float)audioHandler.DurationAudio(clip_nr));


    } 
    
    IEnumerator PlayAndTalkNextClipCoroutine()
    {
        Debug.Log("Next clip Corutine; clip: "+audio_nr);

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


    private void ShowDecisionTree()
    {
        Debug.Log("Show decision tree");

        decisionTree.GetComponent<POV_DecisionTree>().InitiateTree(dataHandler);
        decisionTree.SetActive(true);
        StartCoroutine(PlayExplanationDecisionTree());


    }

    IEnumerator PlayExplanationDecisionTree()
    {
        while (audio_nr < audioHandler.GetAudioClipArrayLength())
        {
            float clip_length = audioHandler.DurationAudio(audio_nr);
            StartCoroutine(PlayAndTalkNextClipCoroutine());
            yield return new WaitForSeconds(clip_length);
        }


        
    }

    public void Yes_Clicked()
    {
        dataHandler.SetFinalDecision(day, true);

        StartCoroutine(ClickOnlyDaysCoroutine());

    }

    public void No_Clicked()
    {

        dataHandler.SetFinalDecision(day, false);
        StartCoroutine(ClickOnlyDaysCoroutine());
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
