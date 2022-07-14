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

    public List<dataPoint> dataPoints;
    public List<dataPoint.categories> categories_filtered_for;
    private GameObject choose_button;
    public List<GameObject> child_nodes;
    public Layer layer;
    public int numberForSorting; //indices for nodes beginns at 1 making root node 1 and its childnodes 11, 12 and evtl. 13

    private bool singular_known = false;
    private bool singular;



    public int NumberDatapoints()
    {
        return dataPoints.Count;
    }

    public bool Singular()
    {
        if (singular_known) return singular;   
        if (dataPoints.Count == 0)
        {
            Debug.Log("datapoints of Node " + gameObject.name + "is empty in Singular()");
            singular_known = true;
            singular = true;
            return true;
        }
        print(dataPoints);
        dataPoint refDp = dataPoints[0];
        foreach (dataPoint dp in dataPoints)
        {
            if (refDp.result != dp.result)
            {
                singular = false;
                singular_known = true;
                return false;
            }

        }
        singular_known = true;
        singular = true;
        return true;
    }





    // Start is called before the first frame update
    void Start()
    {
        
        //InitFrame(2,3, null, null);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void InitFrame(List<dataPoint.categories> filtered_for, List<dataPoint> relevant_datapoints, Layer layer, int numberSort, int number_datapoints_to_left)
    {


        gameObject.name = "Node" + numberSort; //For naming only
        dataPoints = relevant_datapoints;
        categories_filtered_for = filtered_for;
        this.layer = layer;
        this.numberForSorting = numberSort;


        //if node is empty
        if (dataPoints.Count == 0 )
        {
            gameObject.SetActive(false);
            return;
        }


        //place according to number of dps to the left
        transform.localPosition = new Vector3((DecisionTreeHandler.s_max_width / layer.countDps) * number_datapoints_to_left, 0, layer.layerLevel);



        //Placing plate for tennisballs in the frames
        //TODO grid layout does not work with the 3d models we are using so far maybe we just have to place them ourselves #4
        

        layer.AddNode(this.gameObject);
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

        //TODO Scale the frames to appropriete size to the number of tennisballs inside #11
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


        if (!this.Singular())
        {
            //TODO only activate Buttons if layer is ready means when layer above is finished #13
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


            //TODO place button in appropriete location relative to frame #14
        }
    }

    public void ButtonClick_Temp() {
        if (this.choose_button is null) return;
        choose_button.gameObject.SetActive(false);
        create_child_nodes(dataPoint.categories.Temperature);
    }

    public void ButtonClick_Outlook()
    {
        if (this.choose_button is null) return;

        choose_button.gameObject.SetActive(false);
        create_child_nodes(dataPoint.categories.Outlook);
    }
    public void ButtonClick_Humidity()
    {
        if (this.choose_button is null) return;

        choose_button.gameObject.SetActive(false);
        create_child_nodes(dataPoint.categories.Humidity);
    }
    public void ButtonClick_Wind()
    {
        if (this.choose_button is null) return;

        choose_button.gameObject.SetActive(false);
        create_child_nodes(dataPoint.categories.Wind);
    }


    void create_child_nodes(dataPoint.categories filtered)
    {

        Layer next_layer = layer.NextLayer();


        child_nodes = new List<GameObject>();

        List<dataPoint.categories> new_filtered_for = new List<dataPoint.categories>(categories_filtered_for);
        new_filtered_for.Add(filtered);

        int number_datapoints_to_left = layer.GetCountDPsToTheLeftForNextLayer(this);
        int new_dp_to_Left = 0;

        if (filtered == dataPoint.categories.Outlook) {
            GameObject first_child = Instantiate(frame_prefab, transform.parent);
            GameObject second_child = Instantiate(frame_prefab, transform.parent);
            GameObject third_child = Instantiate(frame_prefab, transform.parent);

            List<dataPoint> dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Outlook] == dataPoint.choices_outlook.Sunny.ToString());
            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 1, number_datapoints_to_left + new_dp_to_Left);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Outlook] == dataPoint.choices_outlook.Overcast.ToString());
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 2, number_datapoints_to_left + new_dp_to_Left);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Outlook] == dataPoint.choices_outlook.Rain.ToString());
            third_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 3, number_datapoints_to_left + new_dp_to_Left);
            

            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
            child_nodes.Add(third_child);
        }

        else if (filtered == dataPoint.categories.Temperature)
        {
            GameObject first_child = Instantiate(frame_prefab, transform.parent);
            GameObject second_child = Instantiate(frame_prefab, transform.parent);
            GameObject third_child = Instantiate(frame_prefab, transform.parent);

            List<dataPoint> dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Temperature] == dataPoint.choices_temperature.Hot.ToString());
            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 1, number_datapoints_to_left + new_dp_to_Left);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Temperature] == dataPoint.choices_temperature.Mild.ToString());
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 2, number_datapoints_to_left + new_dp_to_Left);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Temperature] == dataPoint.choices_temperature.Cool.ToString());
            third_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 3, number_datapoints_to_left + new_dp_to_Left);

            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
            child_nodes.Add(third_child);
        }
        else if (filtered == dataPoint.categories.Humidity)
        {
            GameObject first_child = Instantiate(frame_prefab, transform.parent);
            GameObject second_child = Instantiate(frame_prefab, transform.parent);

            List<dataPoint> dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Humidity] == dataPoint.choices_humidity.Normal.ToString());
            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 1, number_datapoints_to_left + new_dp_to_Left);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Humidity] == dataPoint.choices_humidity.High.ToString());
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 2, number_datapoints_to_left + new_dp_to_Left);

            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
        }
        else if (filtered == dataPoint.categories.Wind)
        {
            GameObject first_child = Instantiate(frame_prefab, transform.parent);
            GameObject second_child = Instantiate(frame_prefab, transform.parent);

            List<dataPoint> dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Wind] == dataPoint.choices_wind.Strong.ToString());
            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 1, number_datapoints_to_left + new_dp_to_Left);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Wind] == dataPoint.choices_wind.Weak.ToString());
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 2, number_datapoints_to_left + new_dp_to_Left);

            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
        }

    }

    public bool equals(System.Object o)
    {
#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
        if (this == o) return true;
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast
        if (o == null || GetType() != o.GetType()) return false;
        FrameHandler frame_handler = (FrameHandler) o;
        return frame_handler.numberForSorting == this.numberForSorting;
    }
}
