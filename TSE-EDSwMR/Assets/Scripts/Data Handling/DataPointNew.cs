using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a DataPoint in the DataHandler.
/// It contains the values for each data category, the final category this dp has to be categorized in to (y/n) and an inde
/// </summary>
public class DataPointNew
{

    public readonly Dictionary<string, string> values;

    public readonly bool result;

    public readonly int index;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pvalues">values for all categories string coded</param>
    /// <param name="presult">final category for this dp</param>
    /// <param name="pindex">id for this dp</param>
    public DataPointNew(Dictionary<string, string> pvalues, bool presult, int pindex)
    {
        this.values = pvalues;
        this.result = presult;
        this.index = pindex;
    }

}
