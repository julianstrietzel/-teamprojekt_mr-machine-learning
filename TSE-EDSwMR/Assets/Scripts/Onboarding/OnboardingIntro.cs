using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;


public class OnboardingIntro : MonoBehaviour
{
    [SerializeField] TextMeshPro questionText;
    [SerializeField] GameObject backPlate;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject yesButton;
    [SerializeField] GameObject noButton;
    [SerializeField] string[] questions = new string[AMOUNT_QUESTIONS];

    [SerializeField] AudioHandlerScript audioHandler;
    [SerializeField] GameObject kai;
    [SerializeField] BotAndAudioScript bot;

    public OnboardingOutro outro;

    // Declaration of GameObject used for requestDialog (collecting the tennis balls) and all needed string constants
    public GameObject textRequest_prefab;
    private Dialog requestDialog;

    private const string REQUEST_TITLE = "Collect the box";
    private const string REQUEST_TEXT = "To be able to start, we first need all the tools. To start, please take the box labeled 'MR: Decision Tree' from the shelf and place it on the table. Are you ready?";
    private bool destroyed = false;

    //TO-DO public in Ser.Field.
    public bool[] answers = new bool[AMOUNT_QUESTIONS];

    private RetrobotAnimations kaiAnimations;

    private bool introDone;
    private bool assessmentDone;
    private bool yesPressed;
    private bool buttonPressed;

    private static readonly int AMOUNT_QUESTIONS = 4;
    private int index_question = AMOUNT_QUESTIONS;
    private int audio_nr = 0;

    private const string WELCOMING_MESSAGE = "Hello! My name is Kai. I will guide you on your learning journey on becoming a Decision Tree Champ! Let's start our journey!";
    private const string FIRST_MESSAGE_ASSESSMENT = "Let's find out, how much you already know. Are you ready to start the assessment?";
    private const string FINAL_MESSAGE_ASSESSMENT = "Congratulations for completing the assessment! Now we can recommend a modul for you to create a customized learning experience!";


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        kaiAnimations = kai.GetComponent<RetrobotAnimations>();
        kaiAnimations.InitiateController();
        index_question = 0;

        StartConfiguration();
    }

    /// <summary>
    /// Update is called once per frame 
    /// Handles the order and assesment and starts with the first Welcoming message and ends with opening the Dialog about getting the box
    /// Assessment-Part: If Yes-button is pressed, the answer gets saved in the array as true, vice versa with the No-button
    /// </summary>
    void Update()
    {
        /// Starts Intro segment
        if (!introDone)
        {
            StartCoroutine(StartIntro());
        }
        /// Starts Assessment segment
        else if (assessmentDone)
        {
            Destroy(startButton, 0);
            if (!destroyed)
            {
                yesButton.SetActive(true);
                noButton.SetActive(true);
            }

            if (index_question < AMOUNT_QUESTIONS)
            {
                questionText.text = questions[index_question];

                if (buttonPressed)
                {
                    ReadDialog();

                    if (yesPressed)
                    {
                        answers[index_question] = true;


                    }
                    else
                    {
                        answers[index_question] = false;

                    }
                    SetButtonPressed(false);
                    index_question++;
                }
            }
            /// Starts Getting-The-Box Segment and overbridges to the Hand-Gesture-Segment
            else if (!destroyed)
            {
                StartCoroutine(DestroyAssessment());
                destroyed = true;
            }
        }
    }

    /// <summary>
    /// Sets the start configurations
    /// </summary>
    private void StartConfiguration()
    {

        SetButtonPressed(false);
        SetIntro(false);
        SetAssessmentDone(false);

        yesButton.SetActive(false);
        noButton.SetActive(false);
        startButton.SetActive(false);
    }

    /// <summary>
    /// This method sets the value of the variable introDone
    /// </summary>
    /// <param name="playedIntro"> boolean, true if the Welcoming segment is completed</param>
    private void SetIntro(bool playedIntro)
    {
        introDone = playedIntro;
    }

    /// <summary>
    /// This method sets the value of the variable assessmentDone
    /// </summary>
    /// <param name="playedAssessment">boolean, true if the Assessment segment is completed</param>
    private void SetAssessmentDone(bool playedAssessment)
    {
        assessmentDone = playedAssessment;
    }

    /// <summary>
    /// This method sets the value of the variable buttonPressed was pressed  
    /// </summary>
    /// <param name="boolean">boolean value; if any button is pressed (START, YES, NO) the value true will be given</param>
    private void SetButtonPressed(bool boolean)
    {
        buttonPressed = boolean;
    }

    /// <summary>
    /// This method sets the value which button is pressed
    /// </summary>
    /// <param name="yesIsClicked"> boolen value; true if YES and false if NO </param>
    private void SetYesPressed(bool yesIsClicked)
    {
        yesPressed = yesIsClicked;
    }

    /// <summary>
    /// This method runs if the Start-Button in the assessment is pressed
    /// </summary>
    public void StartButtonPressed()
    {
        SetAssessmentDone(true);
        ReadDialog();
    }
    /// <summary>
    /// This method runs if the Yes-Button in the assessment is pressed
    /// </summary>
    public void YesButtonPressed()
    {
        SetYesPressed(true);
        SetButtonPressed(true);
    }

    /// <summary>
    /// This method runs if the No-Button in the assessment is pressed
    /// </summary>
    public void NoButtonPressed()
    {
        SetYesPressed(false);
        SetButtonPressed(true);
    }

    /// <summary>
    /// This method plays the current audio clip and contains the Talking Animation of the robot
    /// </summary>
    private void ReadDialog()
    {
        bot.PlayClip(audio_nr);
        audio_nr++;
    }

    /// <summary>
    /// This method runs the Intro segment
    /// </summary>
    /// <returns> WaitingMethod that pauses the code for the duration of the audio clip length </returns>
    IEnumerator StartIntro()
    {
        questionText.text = WELCOMING_MESSAGE;
        float clip_length = audioHandler.DurationAudio(audio_nr);
        kaiAnimations.WaveHand();
        ReadDialog();
        SetIntro(true);
        yield return new WaitForSeconds(clip_length);
        StartCoroutine(StartAssessment());
    }

    /// <summary>
    /// This method runs the Assessment segment
    /// </summary>
    /// <returns> WaitingMethod that pauses the code for the duration of the audio clip length </returns>
    IEnumerator StartAssessment()
    {
        questionText.text = FIRST_MESSAGE_ASSESSMENT;
        float clip_length = audioHandler.DurationAudio(audio_nr);
        ReadDialog();
        startButton.SetActive(true);
        yield return new WaitForSeconds(clip_length);
    }

    /// <summary>
    /// This method ends the Assessment segment and destroys GameObjects that are no longer used
    /// Starts the Get-The-Box Segment
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyAssessment()
    {
        destroyed = true;

        questionText.text = FINAL_MESSAGE_ASSESSMENT;

        float clip_length = audioHandler.DurationAudio(audio_nr);

        Destroy(yesButton, 0);
        Destroy(noButton, 0);

        //destroys the questionText GameObject after 2sec
        Destroy(questionText, clip_length);
        Destroy(backPlate, clip_length);


        ReadDialog();

        yield return new WaitForSeconds(clip_length);

        //@LUCA: Wenn du Kai nicht im Bild haben willst 
        ShowRequestDialog();
    }

    /// <summary>
    /// This method opens a dialog for the next interaction.
    /// Dialog asks users to get the box with balls to continue.
    /// </summary>
    private void ShowRequestDialog()
    {
        ReadDialog();
        //opens the request Dialog
        Dialog requestDialog = Dialog.Open(textRequest_prefab, DialogButtonType.Yes, REQUEST_TITLE, REQUEST_TEXT, true);
        requestDialog.OnClosed += OnClose;
    }

    public void OnClose(DialogResult res)
    {
        Destroy(kai); //TODO place where necessary LUCA
        outro.GestureControlIntroduction();
    }

    

   /** public int GetModulRecommondation()
    {
        int module;

        for ( int i = 0; i < answers.Length ; i++ )
        {
            if (answers[i] == false)
            {
                return module = i + 1;
            }
        }
    }
   **/
}
