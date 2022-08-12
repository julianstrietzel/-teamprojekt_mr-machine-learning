using Microsoft.MixedReality.Toolkit.UI;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntropyHandler : MonoBehaviour
{
    FrameHandler parentFrameHandler;
    ToolTip tooltip;
    Dictionary<string, float> gains;

    public void Initalise()
    {
        parentFrameHandler = gameObject.GetComponent<FrameHandler>();
        tooltip = gameObject.GetComponentInChildren<ToolTip>();
        gains = new Dictionary<string, float>();

        foreach (JObject cat in DataHandler.categories)
        {
            if (!parentFrameHandler.categories_filtered_for.Contains(cat.Value<string>("id")))
            {
                gains.Add(cat.Value<string>("id"), getInformationGain(cat.Value<string>("id")));
            }
        }

        float thisEntropy = calculateEntropy(parentFrameHandler.dataPoints);
        tooltip.ToolTipText = "Entropy: " + thisEntropy;
    }

    //Splits the Datapoints into the corresponding lists of categories
    public List<List<DataPointNew>> splitData(string categorie)
    {
        List<List<DataPointNew>> splitted = new List<List<DataPointNew>>();

        JObject categorie_filtered = null;
        foreach (JObject cat in DataHandler.categories)
        {
            if (cat["id"].Value<string>() == categorie) categorie_filtered = cat;
        }

        foreach (string choice in categorie_filtered["choices"].Values<string>())
        {
            splitted.Add(parentFrameHandler.dataPoints.FindAll(e => e.values[categorie] == choice));
        }
        return splitted;
        
    }

    //Calculate Entropy for 2 Classes (Yes and No)
    public float calculateEntropy(List<DataPointNew> split)
    {
        int numDatapoints = split.Count;
        int numberYes = split.FindAll(e => e.result).Count;

        float probabilityYes = numberYes / (float) numDatapoints;
        float probabilityNo = 1 - probabilityYes;

        if (probabilityYes == 1 || probabilityNo == 1) return 0;

        return -1f * (probabilityYes * Mathf.Log(probabilityYes,2) + probabilityNo * Mathf.Log(probabilityNo,2));

    }

    public float calculateInformationGain(float thisEntropy, List<float> childEntropys, List<float> weights)
    {
        if (childEntropys.Count != weights.Count)
        {
            Debug.Log("Ungleicher Count");
            throw new System.ArgumentException("Count should be the same");
        }

        float informationGain = thisEntropy;
        for (int i = 0; i < childEntropys.Count; i++)
        {
            informationGain -= childEntropys[i] * weights[i];
        }

        return informationGain;
    }

    public float getInformationGain(string categorie)
    {
        if(parentFrameHandler.NumberDatapoints() == 0) return 0; //actually unnecessary as it is only invoked, if node is not empty, but good fallback

        List<float> childEntropys = new List<float>();
        List<float> weights = new List<float>();
        float thisEntropy = calculateEntropy(parentFrameHandler.dataPoints);

        List<List<DataPointNew>> splittedData = splitData(categorie);

        foreach (List<DataPointNew> split in splittedData)
        {
            childEntropys.Add(calculateEntropy(split));
            weights.Add(split.Count / parentFrameHandler.dataPoints.Count);
        }

        return calculateInformationGain(thisEntropy, childEntropys, weights);

    }
}
