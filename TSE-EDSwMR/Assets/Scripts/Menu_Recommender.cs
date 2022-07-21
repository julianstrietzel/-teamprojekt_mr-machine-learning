using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Menu_Recommender : MonoBehaviour
{
    //Change that Static highlight the wanted Module
    public static int recommendedModule = 1;

    public GameObject Highlight_Module1;
    public GameObject Highlight_Module2;
    public GameObject Highlight_Module3;
    private Dictionary<int, GameObject> highlightDict = new Dictionary<int, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        Highlight_Module1.SetActive(false);
        Highlight_Module2.SetActive(false);
        Highlight_Module3.SetActive(false);

        highlightDict.Add(1, Highlight_Module1);
        highlightDict.Add(2, Highlight_Module2);
        highlightDict.Add(3, Highlight_Module3);

        highlightDict[recommendedModule].SetActive(true);
      
    }
}
