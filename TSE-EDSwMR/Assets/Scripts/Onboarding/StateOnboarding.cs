using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOnboarding : MonoBehaviour
{
    [SerializeField] AudioHandlerScript audioHandler;
    [SerializeField] GameObject kai;

    private RetrobotAnimations kaiAnimations;
    private int audio_nr = 0;
    private bool intro = true;

    void Start()
    {
        kaiAnimations = kai.GetComponent<RetrobotAnimations>();

        kaiAnimations.InitiateController(); // otherwise NullPointer because Start is called after the first animation is needed 

        //StartCoroutine(PlayFirstMove()); // Kicks off Coroutine

    }

    // Update is called once per frame
    void Update()
    {
        if (intro == true && !audioHandler.isPlaying())
        {

            Debug.Log("intro false");
            intro = false;
           // StartCoroutine(PlayFirstMove());
        }
    }


}
