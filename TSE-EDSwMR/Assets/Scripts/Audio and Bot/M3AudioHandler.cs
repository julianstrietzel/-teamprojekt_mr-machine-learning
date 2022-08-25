using UnityEngine;

/// <summary>
/// This class calls the correct Audiofiles on the M3 (Rebuild Decisiontree)
/// </summary>
public class M3AudioHandler : MonoBehaviour
{
    private BotAndAudioScript botAndAudioScript;

    public void Start()
    {

        botAndAudioScript = GetComponent<BotAndAudioScript>();
    }


    public void PlayIntro()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(0));

        Debug.Log("M3PlayIntroduction", this);
    }

    public void PlaySumUp()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(1));

        Debug.Log("M3PlaySUmUp", this);
    }

    /// <summary>
    /// Is played when the first layer is built, to give an extre thought
    /// </summary>
    public void PlayAdditionalNotes()
    {
        if (botAndAudioScript != null) StartCoroutine(botAndAudioScript.PlayClipCoroutine(2));

        Debug.Log("M3PlayAdditionalNotes", this);

    }

}
