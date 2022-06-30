using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class FrameHandler : MonoBehaviour
{
    public GameObject grid;
    public GameObject YesPrefab;
    public GameObject NoPrefab;
    public GameObject EmptyPrefab;
    public GameObject choose_button_prefab;
    public GameObject frame_prefab;

    private List<dataPoint> dataPoints;
    private List<dataPoint.categories> categories_filtered_for;
    private GameObject choose_button;
    public List<GameObject> child_nodes;
    private int layer;
    private int rightbound;
    private int leftbound;


    // Start is called before the first frame update
    void Start()
    {
        
        //InitFrame(2,3, null, null);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void InitFrame(List<dataPoint.categories> filtered_for, List<dataPoint> relevant_datapoints, int layer, int rightBound, int leftBound)
    {
        this.layer = layer;
        this.rightbound = rightBound;
        this.leftbound = leftBound;

        gameObject.transform.localPosition = new Vector3((rightBound + leftBound) / 2, 0, layer);


        dataPoints = relevant_datapoints;
        categories_filtered_for = filtered_for;
        foreach(Transform plate in grid.transform)
        {
            GameObject.Destroy(plate.gameObject);
        }
        int yes = 0;
        int no = 0;
        foreach(dataPoint dp in relevant_datapoints){
            if(dp.result)
            {
                yes++;
            } else
            {
                no++;
            }
        }
        int height = 4;
        int width = (int)Math.Ceiling(relevant_datapoints.Count / (float) height);
        int none = (int) width * height - yes - no;
        //gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * width / height, 1, 1); //Scales frames to necessary width
        for(int i = 0; i < yes; i++)
        {
            Instantiate(YesPrefab, grid.transform);
        }
        for (int i = 0; i < none; i++)
        {
            Instantiate(EmptyPrefab, grid.transform);
        }
        for (int i = 0; i < no; i++)
        {
            Instantiate(NoPrefab, grid.transform);
        }
        grid.AddComponent<GridLayoutGroup>();


        choose_button = Instantiate(choose_button_prefab, gameObject.transform);
        choose_button.transform.localPosition = new Vector3(.5f, .7f, 0);
        Transform button_collection = choose_button.transform.GetChild(1);
        Transform temp_button = button_collection.transform.GetChild(1);        
        Transform outlook_button = button_collection.transform.GetChild(0);
        Transform hum_button = button_collection.transform.GetChild(2);
        Transform wind_button = button_collection.transform.GetChild(3);
        temp_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed.AddListener(ButtonClick_Temp);
        outlook_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed.AddListener(ButtonClick_Outlook);
        hum_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed.AddListener(ButtonClick_Humidity);
        wind_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed.AddListener(ButtonClick_Wind);

        // outlook_button.GetComponent<Button>().onClick.AddListener(ButtonClick_Outlook); 
        //hum_button.GetComponent<Button>().onClick.AddListener(ButtonClick_Humidity);
        //wind_button.GetComponent<Button>().onClick.AddListener(ButtonClick_Wind);

        if (filtered_for.Contains(dataPoint.categories.Outlook))
        {
            outlook_button.gameObject.SetActive(false);
        }
        if (filtered_for.Contains(dataPoint.categories.Temperature))
        {
            temp_button.gameObject.SetActive(false);
        }
        if (filtered_for.Contains(dataPoint.categories.Humidity))
        {
            hum_button.gameObject.SetActive(false);
        }
        if (filtered_for.Contains(dataPoint.categories.Wind))
        {
            wind_button.gameObject.SetActive(false);
        }

        //TODO place button in appropriete location relative to frame
    }

    public void ButtonClick_Temp() {
        choose_button.gameObject.SetActive(false);
        create_child_nodes(dataPoint.categories.Temperature);
    }

    public void ButtonClick_Outlook()
    {
        choose_button.gameObject.SetActive(false);
        create_child_nodes(dataPoint.categories.Outlook);
    }
    public void ButtonClick_Humidity()
    {
        choose_button.gameObject.SetActive(false);
        create_child_nodes(dataPoint.categories.Humidity);
    }
    public void ButtonClick_Wind()
    {
        choose_button.gameObject.SetActive(false);
        create_child_nodes(dataPoint.categories.Wind);
    }


    void create_child_nodes(dataPoint.categories filtered)
    {
        child_nodes = new List<GameObject>();
        List<dataPoint.categories> new_filtered_for = new List<dataPoint.categories>(categories_filtered_for);
        new_filtered_for.Add(filtered);

        if (filtered == dataPoint.categories.Outlook) {
            GameObject first_child = Instantiate(frame_prefab, gameObject.transform.parent);
            GameObject second_child = Instantiate(frame_prefab, gameObject.transform.parent);
            GameObject third_child = Instantiate(frame_prefab, gameObject.transform.parent);

            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dataPoints.FindAll(
                e => e.values[dataPoint.categories.Outlook] == dataPoint.choices_outlook.Sunny.ToString()), layer -1, leftbound, leftbound + (rightbound -leftbound) / 3);
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dataPoints.FindAll(
                e => e.values[dataPoint.categories.Outlook] == dataPoint.choices_outlook.Overcast.ToString()), layer-1, leftbound + (rightbound - leftbound) / 3, leftbound + 2 * (rightbound - leftbound) / 3);
            third_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dataPoints.FindAll(
                e => e.values[dataPoint.categories.Outlook] == dataPoint.choices_outlook.Rain.ToString()), layer-1, leftbound + 2 * (rightbound - leftbound) / 3, rightbound);

            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
            child_nodes.Add(third_child);
        }

        if (filtered == dataPoint.categories.Temperature)
        {
            GameObject first_child = Instantiate(frame_prefab);
            GameObject second_child = Instantiate(frame_prefab);
            GameObject third_child = Instantiate(frame_prefab);
            first_child.transform.parent = gameObject.transform.parent;
            third_child.transform.parent = gameObject.transform.parent;
            second_child.transform.parent = gameObject.transform.parent;

            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dataPoints.FindAll(
                e => e.values[dataPoint.categories.Temperature] == dataPoint.choices_temperature.Cool.ToString()), layer - 1, leftbound, leftbound + (rightbound - leftbound) / 3);
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dataPoints.FindAll(
                e => e.values[dataPoint.categories.Temperature] == dataPoint.choices_temperature.Mild.ToString()), layer - 1, leftbound + (rightbound - leftbound) / 3, leftbound + 2 * (rightbound - leftbound) / 3);
            third_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dataPoints.FindAll(
                e => e.values[dataPoint.categories.Temperature] == dataPoint.choices_temperature.High.ToString()), layer - 1, leftbound + 2 * (rightbound - leftbound) / 3, rightbound);

            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
            child_nodes.Add(third_child);
        }
        if (filtered == dataPoint.categories.Humidity)
        {
            GameObject first_child = Instantiate(frame_prefab);
            GameObject second_child = Instantiate(frame_prefab);
            first_child.transform.parent = gameObject.transform.parent;
            second_child.transform.parent = gameObject.transform.parent;

            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dataPoints.FindAll(
                e => e.values[dataPoint.categories.Humidity] == dataPoint.choices_humidity.Normal.ToString()), layer - 1, leftbound, (leftbound + rightbound) / 2);
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dataPoints.FindAll(
                e => e.values[dataPoint.categories.Humidity] == dataPoint.choices_humidity.High.ToString()), layer - 1, (leftbound + rightbound) / 2, rightbound);


            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
        }
        if (filtered == dataPoint.categories.Wind)
        {
            GameObject first_child = Instantiate(frame_prefab);
            GameObject second_child = Instantiate(frame_prefab);
            first_child.transform.parent = gameObject.transform.parent;
            second_child.transform.parent = gameObject.transform.parent;

            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dataPoints.FindAll(
                e => e.values[dataPoint.categories.Wind] == dataPoint.choices_wind.Weak.ToString()), layer - 1, leftbound, (leftbound + rightbound) / 2);
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dataPoints.FindAll(
                e => e.values[dataPoint.categories.Wind] == dataPoint.choices_wind.Strong.ToString()), layer - 1, (leftbound + rightbound) / 2, rightbound);

            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
        }

    }
}
