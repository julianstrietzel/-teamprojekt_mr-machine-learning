using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsScript : MonoBehaviour
{

    [SerializeField] GameObject hint_prefab;

    private string hint_begining;
    private string hint_ending;

    public void Hint()
    {
        string message = "here is an explanation";
        Dialog.Open(hint_prefab, DialogButtonType.OK, "Hint", message, true);
    }


}
