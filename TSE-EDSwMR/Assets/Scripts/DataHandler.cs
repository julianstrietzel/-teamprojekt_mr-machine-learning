    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



public class DataHandler : MonoBehaviour 
{
    public static List<DataPointNew> data = new List<DataPointNew>();
    public GameObject decisionTree;
    public static JArray categories;

    void Start()
    {
        data.Clear();

        string json = File.ReadAllText(Application.dataPath + "/Data/tse_tennis.json");
        JObject jobject = JObject.Parse(json);
        categories = (JArray)jobject["categories"];
        foreach (JObject dp in jobject["datapoints"])
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach(JObject cat in categories)
            {
                string catId = cat.Value<string>("id");
                values.Add(catId, dp.Value<String>(catId));
            }
            data.Add(new DataPointNew(values, dp.Value<bool>("result"), dp.Value<int>("index")));
        }

        decisionTree.GetComponent<DecisionTreeHandler>().OnDataHandlerInit(); 
    }

}
