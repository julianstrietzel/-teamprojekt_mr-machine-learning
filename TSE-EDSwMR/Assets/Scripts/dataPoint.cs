using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Obsolete("Use DatapointNew instead")]
public class dataPoint
{

    
    public enum categories
    {
        Outlook,
        Temperature,
        Humidity,
        Wind
    }


    public class Categories
    {
        
    }

    

   // public static Dictionary<categories, object> choices_categories = new Dictionary<categories, object>();
    //TODO working as JSON structure

    public enum choices_outlook
    {
        Sunny, 
        Overcast, 
        Rain
    }

    public enum choices_temperature
    {
        Hot, 
        Mild, 
        Cool
    }
    
    public enum choices_humidity
    {
        High,
        Normal
    }

    public enum choices_wind
    {
        Strong,
        Weak
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
