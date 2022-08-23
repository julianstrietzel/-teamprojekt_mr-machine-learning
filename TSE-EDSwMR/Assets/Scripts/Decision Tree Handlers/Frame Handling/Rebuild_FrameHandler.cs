using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebuild_FrameHandler : FrameHandler
{
    

    public void DestroyThisPart()
    {
        foreach (GameObject node in child_nodes)
        {
            node.GetComponent<Rebuild_FrameHandler>().DestroyThisPart();
        }
        Destroy(gameObject);
    }
    

}
