using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Sometimes leads to a problem where the bot does not talk,
/// this is because the animation function is called before the RetrobotAnimation Start() is called.
/// Add the AudioFiles to the AudioHandler's Audio Clip Array
/// </summary>
public class BotAndAudioScript : MonoBehaviour
{
    [SerializeField] AudioHandlerScript audioHandler;
    [SerializeField] RetrobotAnimations retrobotAnimations;


    // has to be called over StartCoroutine()
    public IEnumerator PlayClipCoroutine(int clip_nr)
    {
        audioHandler.PlayAudioClipNr(clip_nr);
        yield return retrobotAnimations.TalkForCoroutine(audioHandler.DurationAudio(clip_nr));
    }

    /// <summary>
    /// Plays the clip from the AudioHandler with the given index number, while the bot does a talking animation.
    /// </summary>
    /// <param name="clip_nr"></param>
    public void PlayClip(int clip_nr)
    {
        audioHandler.PlayAudioClipNr(clip_nr);
        StartCoroutine(retrobotAnimations.TalkForCoroutine(audioHandler.DurationAudio(clip_nr)));
    }


}
