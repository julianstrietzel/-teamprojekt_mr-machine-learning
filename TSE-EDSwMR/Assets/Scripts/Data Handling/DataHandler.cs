    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



/// <summary>
/// Class for handling the Data from a given Json File in the correct format.
/// Needs a DecisionTree with Handler as Reference Tree to init and a correct specified path to json file.
/// 
/// </summary>

public class DataHandler : MonoBehaviour 
{
    [SerializeField] private string jsonPath = "Data/tse_tennis";
    [HideInInspector] public static List<DataPointNew> data = new List<DataPointNew>();
    [HideInInspector] public static JArray categories;
    [SerializeField] private GameObject decisionTree;


    /// <summary>
    /// Inits the Data from json and calls OnDatahandlerInit in the referenced DecisionTree.
    /// </summary>
    void Start()
    {
        data.Clear();
        JObject jobject = JObject.Parse(Resources.Load<TextAsset>(jsonPath).ToString());
        categories = (JArray)jobject["categories"];
        foreach (JObject dp in jobject["datapoints"])
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach(JObject cat in categories)
            {
                string catId = cat.Value<string>("id");
                values.Add(catId, dp.Value<string>(catId));
            }
            data.Add(new DataPointNew(values, dp.Value<bool>("result"), dp.Value<int>("index")));
        }
        decisionTree.GetComponent<DecisionTreeHandler>().OnDataHandlerInit(); 
    }

}
