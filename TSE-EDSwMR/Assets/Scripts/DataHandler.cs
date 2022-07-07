    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataHandler : MonoBehaviour 
{
    public static List<dataPoint> data = new List<dataPoint>();
    public GameObject decisionTree;

    void Start()
    {
        data.Clear();

        data.Add(new dataPoint("Sunny",     "Hot",  "High", "Weak",     false,  1));
        data.Add(new dataPoint("Sunny",     "Hot",  "High", "Strong",   false,  2));
        data.Add(new dataPoint("Overcast",  "Hot",  "High", "Weak",     true,   3));
        data.Add(new dataPoint("Rain",      "Mild", "High", "Weak",     true,   4));
        data.Add(new dataPoint("Rain",      "Cool", "Normal", "Weak",   true,   5));
        data.Add(new dataPoint("Rain",      "Cool", "Normal", "Strong", false,  6));
        data.Add(new dataPoint("Overcast",  "Cool", "Normal", "Strong", true,   7));
        data.Add(new dataPoint("Sunny",     "Mild", "High", "Weak",     false,  8));
        data.Add(new dataPoint("Sunny",     "Cool", "Normal", "Weak",   true,   9));
        data.Add(new dataPoint("Rain",      "Mild", "Normal", "Weak",   true,   10));
        data.Add(new dataPoint("Sunny",     "Mild", "Normal", "Strong", true,   11));
        data.Add(new dataPoint("Overcast",  "Mild", "High", "Strong",   true,   12));
        data.Add(new dataPoint("Overcast",  "Hot",  "Normal", "Weak",   true,   13));
        data.Add(new dataPoint("Rain",      "Mild", "High", "Weak",     false,  14));

        decisionTree.GetComponent<DecisionTreeHandler>().OnDataHandlerInit(); 
    }

}
