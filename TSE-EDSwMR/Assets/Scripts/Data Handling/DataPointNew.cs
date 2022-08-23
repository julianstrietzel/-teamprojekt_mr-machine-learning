using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPointNew
{

    public readonly Dictionary<string, string> values;

    public readonly bool result;

    public readonly int index;


   
    public DataPointNew(Dictionary<string, string> pvalues, bool presult, int pindex)
    {
        this.values = pvalues;
        this.result = presult;
        this.index = pindex;
    }

}
