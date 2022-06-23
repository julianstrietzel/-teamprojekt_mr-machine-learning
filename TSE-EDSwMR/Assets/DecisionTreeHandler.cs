using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTreeHandler : MonoBehaviour
{

    public dataHandler data;

    public GameObject frame_prefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject root = Instantiate(frame_prefab);
        root.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
