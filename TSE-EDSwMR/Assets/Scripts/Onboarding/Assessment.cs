using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Assessment : MonoBehaviour
{
    public static readonly int AMOUNT_QUESTIONS = 4;

    public TextMeshPro questionText;

    public string[] questions = new string[AMOUNT_QUESTIONS];
    public int[] answers = new int[AMOUNT_QUESTIONS];


    void Start()
    {
        GenerateQuestion();
    }

    void GenerateQuestion()
    {
        questions[0] = "How are you?";
        questions[1] = "Are you ready?";
        questions[2] = "Finished?";
        questions[3] = "Boogah";
    }

    public void YesButtonPressed()
    {
        GetNextQuestion(true);
    }

    public void NoButtonPressed()
    {
        GetNextQuestion(false);
    }

    //Goes through saved questions in array and saves answer
    // bool boolean: if yes button pressed then true, if no button pressed then false
    void GetNextQuestion(bool yesPressed)
    {
        int i = 0;
        while (i < AMOUNT_QUESTIONS)
        {
            if (yesPressed)
            {
                answers[i] = 1;
            }

            else
            {
                answers[i] = 0;
            }
        }

        questionText.text = questions[i];
        i++;
    }

 }


