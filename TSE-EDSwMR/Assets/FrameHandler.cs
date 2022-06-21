using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class FrameHandler : MonoBehaviour
{
    public GameObject grid;
    public GameObject YesPrefab;
    public GameObject NoPrefab;
    public GameObject EmptyPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
        initFrame(2,3);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void initFrame(int yes, int no)
    {
        int length = (int) Math.Ceiling(Math.Sqrt(yes + no));
        int none = (int) Math.Pow(length, 2) - yes - no;
        for(int i = 0; i < yes; i++)
        {
            Instantiate(YesPrefab).transform.parent = grid.transform;
        }
        for (int i = 0; i < none; i++)
        {
            Instantiate(EmptyPrefab).transform.parent = grid.transform;
        }
        for (int i = 0; i < no; i++)
        {
            Instantiate(NoPrefab).transform.parent = grid.transform;
        }
        grid.AddComponent<GridLayoutGroup>();
        

    //Instantiate

    }
}
