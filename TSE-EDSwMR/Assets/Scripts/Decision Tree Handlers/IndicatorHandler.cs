using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Visualizes the number of Tennisballs on the Frame
/// </summary>
public class IndicatorHandler : MonoBehaviour
{
    //prefabs for the yes and no plates
    public GameObject prefabYes;
    public GameObject prefabNo; 


    public void Visualize(int yes, int no)
    {
        DestroyChildrenIndicators();
        transform.localScale = Vector3.one * .25f;
        transform.localPosition = new Vector3(0, .17f, 0);
        for (int i = 0; i < yes; i++)
        {
            Instantiate(prefabYes, this.transform).transform.localPosition = new Vector3(0.25f * i + .15f, 0, 0);

        }
        for (int i = 0; i < no; i++)
        {
            Instantiate(prefabNo, this.transform).transform.localPosition = new Vector3(-.25f * i - .15f, 0, 0);

        }

        transform.GetChild(0).GetComponent<TextMeshPro>().text = yes + "x";
        transform.GetChild(1).GetComponent<TextMeshPro>().text = no + "x"; 
        if (yes == 0) transform.GetChild(0).GetComponent<TextMeshPro>().text = "";
        if (no == 0) transform.GetChild(1).GetComponent<TextMeshPro>().text = "";
    }

    /// <summary>
    /// Destroys any possible leftovers on the frame
    /// </summary>
    private void DestroyChildrenIndicators()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Text_yes" || child.gameObject.name == "Text_no") continue;
            GameObject.Destroy(child.gameObject);
        }
    }
}
