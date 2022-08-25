using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4AudioHandler : MonoBehaviour

{

    private BotAndAudioScript botAndAudioScript;
    // Use this class to play audios for explanation of M4 Information Gain and Entropy

    public void Start()
    {
     
        botAndAudioScript = GetComponent<BotAndAudioScript>();
    }

    public void ExplainEntropy()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(0));

        Debug.Log("M4 Explain Entropy audio", this);
    }
        
    public void ExplainIG()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(1));

        Debug.Log("M4 Explain Information Gain", this);
    }

    public void ExplainID3()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(2));

        Debug.Log("M4 Explain ID 3 play", this);
    }


}
