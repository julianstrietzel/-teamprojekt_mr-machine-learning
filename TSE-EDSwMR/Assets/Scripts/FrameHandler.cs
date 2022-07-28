using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
public class FrameHandler : MonoBehaviour
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
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
    private bool button_pressed = false;
    private bool activated = false;
    public float space_for_buttons_normed = 4;
    private Color color;
     



    public int NumberDatapoints()
    {
        return dataPoints.Count;
    }

    /// <summary>
    /// For layer activation: Next layer can be activated if every node in the previous layer is ready.
    /// ready means:
    /// The node was activated (if singular this does not do anything)
    /// AND 
    /// the node is singular    OR      the button was pressedd
    /// </summary>
    /// <returns></returns>
    public bool IsReady()
    {

        return activated && (Singular() || button_pressed);
        
    }

    /// <summary>
    /// Singular indicates whether the datapoints are all of one kind and so the tree is ready with learning in this leafe. 
    /// </summary>
    /// <returns>Is this node singular?</returns>
    public bool Singular()
    {
        if (singular_known) return singular;   
        dataPoint refDp = null;
        foreach (dataPoint dp in dataPoints)
        {
            if (!(refDp == null) && refDp.result != dp.result)
            {
                singular = false;
                singular_known = true;
                return false;
            }
            refDp = dp;
        }
        singular_known = true;
        singular = true;
        return true;
    }


    /// <summary>
    /// This function Initiates a frame. It is to be called from its parent node when the user chooses the category to filter for. 
    /// It creates the frame, initializes its variables and datapoints, and calls the visualizing of the content
    /// </summary>
    /// <param name="filtered_for">All the categories that a previous node has filtered for</param>
    /// <param name="relevant_datapoints">All the leftover datapoints this part of the tree has to handle</param>
    /// <param name="layer">the level in which this node is home</param>
    /// <param name="numberSort">an integer where each digit gives the position in a layer. It creates a order between all nodes. Example: 123: The third child of the second child of the root.</param>
    /// <param name="number_datapoints_to_left">Gives the number of datapoints in this layer to the left of this node. All previously finally filtered or singular nodes are calculated out of this.</param>
    public void InitFrame(List<dataPoint.categories> filtered_for, List<dataPoint> relevant_datapoints, Layer layer, int numberSort, int number_datapoints_to_left, Color prev_color)
    {
        gameObject.name = "Node" + numberSort; //For naming only
        dataPoints = relevant_datapoints;
        categories_filtered_for = filtered_for;
        this.layer = layer;
        this.numberForSorting = numberSort;


        //colorcoding:
        Transform frame = transform.GetChild(0);
        frame.GetChild(1).GetComponent<Renderer>().material.color = prev_color;
        color = DecisionTreeHandler.RandomColor();
        frame.GetChild(0).GetComponent<Renderer>().material.color = color;
        frame.GetChild(2).GetComponent<Renderer>().material.color = color;
        frame.GetChild(3).GetComponent<Renderer>().material.color = color;


        //if node is empty deactivate it. It will also not be added to any Layer so nothing is referencing on this
        if (dataPoints.Count == 0 )
        {
            gameObject.SetActive(false);
            return;
        }


        //place according to number of dps to the left
        transform.localPosition = new Vector3((DecisionTreeHandler.s_max_width / layer.countDps) * number_datapoints_to_left, 0, layer.layerLevel * (-1.2f));
        if(layer.layerLevel == 0) { transform.localPosition = new Vector3(DecisionTreeHandler.s_max_width / 4, 0, 0); }//special handling for root node

        //Scale X Axis according to number of tennisballs
        float new_x_scale = ((float) Math.Ceiling(((float) relevant_datapoints.Count) /  space_for_buttons_normed) / space_for_buttons_normed);
        Debug.Log(new_x_scale + " " + numberForSorting  + " " + transform.childCount);
        Vector3 localScale = transform.GetChild(0).localScale;
        transform.GetChild(0).localScale = new Vector3(new_x_scale, localScale.y, localScale.z);

        layer.AddNode(gameObject);

        //Placing plate for tennisballs in the frame
        transform.GetChild(1).GetComponent<IndicatorHandler>().Visualize(relevant_datapoints.FindAll(e => e.result).Count, relevant_datapoints.FindAll(e => !e.result).Count);
    }


    public void Activate()
    {
        activated = true;
        if (Singular()) return;

        choose_button = Instantiate(choose_button_prefab, gameObject.transform);
        button_pressed = false;
        choose_button.transform.localPosition = new Vector3(.5f, .7f, 0);
        Transform button_collection = choose_button.transform.GetChild(1);

        Transform outlook_button = button_collection.transform.GetChild(0);
        Transform temp_button = button_collection.transform.GetChild(1);
        Transform hum_button = button_collection.transform.GetChild(2);
        Transform wind_button = button_collection.transform.GetChild(3);
        outlook_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed.AddListener(ButtonClick_Outlook);
        temp_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed.AddListener(ButtonClick_Temp);
        hum_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed.AddListener(ButtonClick_Humidity);
        wind_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed.AddListener(ButtonClick_Wind);

        if (categories_filtered_for.Contains(dataPoint.categories.Outlook))
        {
            outlook_button.gameObject.SetActive(false);
        }
        if (categories_filtered_for.Contains(dataPoint.categories.Temperature))
        {
            temp_button.gameObject.SetActive(false);
        }
        if (categories_filtered_for.Contains(dataPoint.categories.Humidity))
        {
            hum_button.gameObject.SetActive(false);
        }
        if (categories_filtered_for.Contains(dataPoint.categories.Wind))
        {
            wind_button.gameObject.SetActive(false);
        }


        //TODO place button in appropriete location relative to frame #14
        
    }

    public void ButtonClick_Temp() {
        if (this.choose_button is null) return;
        choose_button.gameObject.SetActive(false);
        Create_child_nodes(dataPoint.categories.Temperature);
    }

    public void ButtonClick_Outlook()
    {
        if (this.choose_button is null) return;

        choose_button.gameObject.SetActive(false);
        Create_child_nodes(dataPoint.categories.Outlook);
    }
    public void ButtonClick_Humidity()
    {
        if (this.choose_button is null) return;

        choose_button.gameObject.SetActive(false);
        Create_child_nodes(dataPoint.categories.Humidity);
    }
    public void ButtonClick_Wind()
    {
        if (this.choose_button is null) return;

        choose_button.gameObject.SetActive(false);
        Create_child_nodes(dataPoint.categories.Wind);
    }


    void Create_child_nodes(dataPoint.categories filtered)
    {
        button_pressed = true;

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
            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 1, number_datapoints_to_left + new_dp_to_Left, color);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Outlook] == dataPoint.choices_outlook.Overcast.ToString());
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 2, number_datapoints_to_left + new_dp_to_Left, color);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Outlook] == dataPoint.choices_outlook.Rain.ToString());
            third_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 3, number_datapoints_to_left + new_dp_to_Left, color);
            

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
            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 1, number_datapoints_to_left + new_dp_to_Left, color);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Temperature] == dataPoint.choices_temperature.Mild.ToString());
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 2, number_datapoints_to_left + new_dp_to_Left, color);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Temperature] == dataPoint.choices_temperature.Cool.ToString());
            third_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 3, number_datapoints_to_left + new_dp_to_Left, color);

            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
            child_nodes.Add(third_child);
        }
        else if (filtered == dataPoint.categories.Humidity)
        {
            GameObject first_child = Instantiate(frame_prefab, transform.parent);
            GameObject second_child = Instantiate(frame_prefab, transform.parent);

            List<dataPoint> dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Humidity] == dataPoint.choices_humidity.Normal.ToString());
            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 1, number_datapoints_to_left + new_dp_to_Left, color);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Humidity] == dataPoint.choices_humidity.High.ToString());
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 2, number_datapoints_to_left + new_dp_to_Left, color);

            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
        }
        else if (filtered == dataPoint.categories.Wind)
        {
            GameObject first_child = Instantiate(frame_prefab, transform.parent);
            GameObject second_child = Instantiate(frame_prefab, transform.parent);

            List<dataPoint> dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Wind] == dataPoint.choices_wind.Strong.ToString());
            first_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 1, number_datapoints_to_left + new_dp_to_Left, color);
            new_dp_to_Left += dps.Count;
            dps = dataPoints.FindAll(e => e.values[dataPoint.categories.Wind] == dataPoint.choices_wind.Weak.ToString());
            second_child.GetComponent<FrameHandler>().InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + 2, number_datapoints_to_left + new_dp_to_Left, color);

            child_nodes.Add(first_child);
            child_nodes.Add(second_child);
        }
        layer.ListenerNodeGeneratesChildren();

    }

    override
    public bool Equals(System.Object o)
    {
#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
        if (this == o) return true;
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast
        if (o == null || GetType() != o.GetType()) return false;
        FrameHandler frame_handler = (FrameHandler) o;
        return frame_handler.numberForSorting == this.numberForSorting;
    }
}
