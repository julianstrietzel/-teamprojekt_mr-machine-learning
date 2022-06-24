using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTreeHandler : MonoBehaviour
{

    public DataHandler data;

    public GameObject frame_prefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject root = new GameObject();
        root.transform.parent = transform;
        root.GetComponent<FrameHandler>().InitFrame(new List<dataPoint.categories>(), DataHandler.data);

        //TODO place the root frame where appropriete according to scene understanding
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
