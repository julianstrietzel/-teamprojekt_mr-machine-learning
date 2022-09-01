using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This provides easy access to the retrobot animations.
/// Needs a retrobot (prefab) to be attached to it. 
/// </summary>
public class RetrobotAnimations : MonoBehaviour
{

    /*    public void DoAction() {        
        retrobotController.DoAction(actions.options[actions.value].text);
    }*/

    /*
     * Needed methods:in Retrobot COntroller  DoAction, SetIdleState
     */

    [SerializeField] GameObject retrobot;
    private RetrobotController controllerRetrobot;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("RetroBot animations Start()");
        InitiateController();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitiateController()
    {
        if (controllerRetrobot == null)
        {
            controllerRetrobot = retrobot.GetComponent<RetrobotController>();
        }
    }


    public IEnumerator TalkForCoroutine(float seconds)
    {
        StartTalking();
        yield return new WaitForSeconds(seconds);
        SetIsTalking(false);

    }
    public IEnumerator TalkAndPresentLeftCoroutine(float seconds)
    {
        Debug.Log("Talk and present left Corutine Retrobot animations");
        StartTalking();
        yield return new WaitForSeconds(seconds);
        PresentLeftAnimation();

    }


    public void TalkingAnimation()
    {
        controllerRetrobot.DoAction("Do_Talking_Head_Hands_Body");
    }

    public void PresentLeftAnimation()
    {
        Debug.Log("PresentLeftAnimation");

        controllerRetrobot.DoAction("Do_Present_Left");
    }

    public void WaveHand()
    {
        controllerRetrobot.DoAction("Do_Wave_Simple");
    }

    public void SetIsTalking(bool stillTalking)
    {
        if(controllerRetrobot.animator != null) controllerRetrobot.animator.SetBool("Is_Talking", stillTalking);
    }
    public bool GetIsTalking()
    {
        return controllerRetrobot.animator.GetBool("Is_Talking");
    }

    public void StartTalking()
    {
        SetIsTalking(true);
        TalkingAnimation();
    }

    public void StopTalking()
    {
        SetIsTalking(false);
        
    }

}
