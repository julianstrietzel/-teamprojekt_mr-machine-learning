using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Addes Rebuild Functionality to Frame
/// </summary>
public class Rebuild_FrameHandler : FrameHandler
{
    
    /// <summary>
    /// Destroys all children
    /// </summary>
    public void DestroyThisPart()
    {
        foreach (GameObject node in child_nodes)
        {
            node.GetComponent<Rebuild_FrameHandler>().DestroyThisPart();
        }
        Destroy(gameObject);
    }
    

}
