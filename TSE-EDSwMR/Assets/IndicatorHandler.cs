using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IndicatorHandler : MonoBehaviour
{
    public GameObject prefabYes;
    public GameObject prefabNo; 


    public void Visualize(int yes, int no)
    {

        Debug.Log("The visualize has the following points: " + yes + " no "+ no);
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

    }

    private void DestroyChildrenIndicators()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Text_yes" || child.gameObject.name == "Text_no") continue;
            GameObject.Destroy(child.gameObject);
        }
    }
}
