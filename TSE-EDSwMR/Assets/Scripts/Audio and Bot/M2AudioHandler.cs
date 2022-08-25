using UnityEngine;

public class M2AudioHandler : MonoBehaviour
{
    private BotAndAudioScript botAndAudioScript;

    public void Start()
    {

        botAndAudioScript = GetComponent<BotAndAudioScript>();
    }

    public void PlayIntroduction()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(0));

        Debug.Log("M2PlayIntroduction", this);
    }

    public void PlayRootExplanation()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(1));

        Debug.Log("M2PlayRootExplanation", this);

    }

    public void PlayFirstLayerExplanation()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(2));
        Debug.Log("M2 First Layer Explanation", this);
    }

    public void PlaySingularNodeExplanation()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(3));
        Debug.Log("M2 Singular Node Explanation", this);
    }



    public void PlaySumUp()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(4));
        Debug.Log("M2 Sum Up Audio Played", this);
    }
}
