using Microsoft.MixedReality.Toolkit.UI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Handles Entropy information on frames
/// Calculates from num of yes and no dps in this frame
/// </summary>
public class EntropyHandler : MonoBehaviour
{
    FrameHandler parentFrameHandler;
    ToolTip tooltip;
    Dictionary<string, float> gains;
    Dictionary<string, TextMesh> TextObjects;
    float thisEntropy;

    public void Initalise()
    {
        parentFrameHandler = gameObject.GetComponent<FrameHandler>();
        tooltip = gameObject.GetComponentInChildren<ToolTip>();
        thisEntropy = calculateEntropy(parentFrameHandler.dataPoints);
        tooltip.ContentScale = 1.89f;
        tooltip.FontSize = 47;
        gains = new Dictionary<string, float>();

        foreach (JObject cat in DataHandler.categories)
        {
            if (!(parentFrameHandler.categories_filtered_for.Contains(cat.Value<string>("id"))))
            {
                gains.Add(cat.Value<string>("id"), getInformationGain(cat.Value<string>("id")));
            } else
            {
                gains.Add(cat.Value<string>("id"), 2);
            }
        }
        
        tooltip.ToolTipText = "Entropy: " + Math.Round(thisEntropy, 3);
    }

    /// <summary>
    /// Updates the text representation of the Informationgain on the buttons
    /// </summary>
    /// <param name="button"> the button where the Informationgain should be updated</param> 
    public void updateButton(GameObject button)
    {
        GameObject textCollection = button.transform.GetChild(2).GetChild(1).gameObject;

        float maxGain = 0f;
        string key_maxGain = null;
        foreach(KeyValuePair<string, float> kvpair in gains)
        {
            if(kvpair.Value > maxGain && kvpair.Value < 1.9f)
            {
                maxGain = kvpair.Value;
                key_maxGain = kvpair.Key;
            }
        }
        for (int i = 0; i < textCollection.transform.childCount; i++)
        {
            GameObject textObject = textCollection.transform.GetChild(i).gameObject;
            TextMeshPro text = textObject.GetComponent<TextMeshPro>();
            if (gains[text.name] == 2)
            {
                text.text = "already chosen";
            }
            else
            {
                text.text = "Information Gain: " + Math.Round(gains[text.name], 3);
                if(text.name == key_maxGain)
                {
                    text.fontStyle = FontStyles.Bold;
                    text.fontStyle = FontStyles.Underline;
                }
            }

        }
    }

    /// <summary>
    /// Splits the Datapoints into the corresponding lists of categories
    /// </summary>
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

    /// <summary>
    /// Calculate Entropy for 2 Classes (Yes and No)
    /// </summary>
    /// <param name="split">Datapoints for Entropy calculation</param>
    /// <returns>Entropy of the split</returns>
    public float calculateEntropy(List<DataPointNew> split)
    {
        float numDatapoints = split.Count;
        if (numDatapoints == 0)
        {
            return 0;
        }
        
        int numberYes = split.FindAll(e => e.result).Count;

        float probabilityYes = numberYes / numDatapoints;
        float probabilityNo = 1 - probabilityYes;

        if (probabilityYes == 1 || probabilityNo == 1) return 0;

        return -1f * (probabilityYes * Mathf.Log(probabilityYes,2) + probabilityNo * Mathf.Log(probabilityNo,2));

    }
    /// <summary>
    /// Calculates the Information Gain from this Frame to the child Frames
    /// </summary>
    /// <param name="childEntropys">Entropys of the child nodes</param>
    /// <param name="weights">Weights of the child nodes</param>
    /// <returns>Information Gain for the child Nodes</returns>
    /// <exception cref="System.ArgumentException"></exception>
    public float calculateInformationGain(List<float> childEntropys, List<float> weights)
    {
        if (childEntropys.Count != weights.Count)
        {
            throw new System.ArgumentException("Count should be the same");
        }
        float informationGain = thisEntropy;
        for (int i = 0; i < childEntropys.Count; i++)
        {
            informationGain -= childEntropys[i] * weights[i];
        }
        
        return informationGain;
    }

    /// <summary>
    /// Creates Lists necessary for calculateInformationGain and calls that
    /// </summary>
    /// <param name="categorie">categorie to split</param>
    /// <returns>Informationgain</returns>
    public float getInformationGain(string categorie)
    {
        if(parentFrameHandler.NumberDatapoints() == 0) return 0; //actually unnecessary as it is only invoked, if node is not empty, but good fallback

        List<float> childEntropys = new List<float>();
        List<float> weights = new List<float>();

        List<List<DataPointNew>> splittedData = splitData(categorie);

        foreach (List<DataPointNew> split in splittedData)
        {
            childEntropys.Add(calculateEntropy(split));
            float splitCount = split.Count;
            weights.Add(splitCount / parentFrameHandler.dataPoints.Count);
        }
        return calculateInformationGain(childEntropys, weights);

    }
}
