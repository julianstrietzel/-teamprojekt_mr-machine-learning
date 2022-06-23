using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataPoint
{
    public enum categories {
        Outlook,
        Temperature,
        Humidity,
        Wind
    }

    public Dictionary<categories, string> values;

    public bool result;

    public int day;
    
    public dataPoint(string outlook, string temp, string hum, string wind, bool result, int day)
    {
        values  = new Dictionary<categories, string>();
        values[categories.Outlook] = outlook;
        values[categories.Temperature] = temp;
        values[categories.Humidity] = hum;
        values[categories.Wind] = wind; 
        this.result = result;
        this.day = day;
    }
   
    
}
