using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Newtonsoft.Json.Linq;
using Microsoft.MixedReality.Toolkit.UI;


/// <summary>
/// This class Handles everything there is to handle for a normal Frame. It is base class for the Rebuild Frame used in M3 and M4.
/// </summary>
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
public class FrameHandler : MonoBehaviour
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    //prefabs
    public GameObject choose_button_prefab;
    public GameObject frame_prefab;

    public List<DataPointNew> dataPoints;
    public List<string> categories_filtered_for;
    private GameObject choose_button;
    protected List<GameObject> child_nodes;
    protected Layer layer;
    public int numberForSorting; //indices for nodes beginns at 1 making root node 1 and its childnodes 11, 12 and evtl. 13

    //status
    private bool singular_known = false; //optimizing calls on singular
    private bool singular;
    private bool button_pressed = false;
    private bool activated = false;

    public float space_for_buttons_normed = 2;
    private Color color;
    private GameObject categorieIcon;

    public bool isEntropyFrame = false;




    
    /// <summary>
    /// Returns the Number of DPs in this Frame
    /// </summary>
    /// <returns></returns>
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
        DataPointNew refDp = null;
        foreach (DataPointNew dp in dataPoints)
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
    public void InitFrame(List<string> filtered_for, List<DataPointNew> relevant_datapoints, Layer layer, int numberSort, int number_datapoints_to_left, Color prev_color)
    {
        gameObject.name = "Node" + numberSort; //For naming only
        dataPoints = relevant_datapoints;
        categories_filtered_for = filtered_for;
        this.layer = layer;
        this.numberForSorting = numberSort;



        //if node is empty deactivate it. It will also not be added to any Layer so nothing is referencing on this
        if (dataPoints.Count == 0 )
        {
            gameObject.SetActive(false);
            return;
        }

        //colorcoding:
        Transform frame = transform.GetChild(0);
        frame.GetChild(1).GetComponent<Renderer>().material.color = prev_color;
        color = Singular() ? Color.white : DecisionTreeHandler.RandomColor(); //Highlighting singular nodes
        frame.GetChild(0).GetComponent<Renderer>().material.color = color;
        frame.GetChild(2).GetComponent<Renderer>().material.color = color;
        frame.GetChild(3).GetComponent<Renderer>().material.color = color;
       
        
        //Init Entropyhandler
        EntropyHandler eHandler = gameObject.GetComponent<EntropyHandler>();
        isEntropyFrame = eHandler != null;
        if (isEntropyFrame) eHandler.Initalise();
        

        //place according to number of dps to the left
        transform.localPosition = new Vector3((DecisionTreeHandler.s_max_width / layer.countDps) * number_datapoints_to_left, 0, layer.layerLevel * (-1.2f));
        if(layer.layerLevel == 0) { transform.localPosition = new Vector3(DecisionTreeHandler.s_max_width / 4, 0, 0); }//special handling for root node

        //Scale X Axis according to number of tennisballs
        float new_x_scale = (float)Math.Ceiling((float)relevant_datapoints.Count / (float)4f) / 4f;
        Vector3 localScale = transform.GetChild(0).localScale;
        transform.GetChild(0).localScale = new Vector3(new_x_scale, localScale.y, localScale.z);


        layer.AddNode(gameObject);

        //Placing plate for each tennisball on the frame, by calling indicator handler on the indicator component of frame prefab
        transform.GetChild(1).GetComponent<IndicatorHandler>().Visualize(relevant_datapoints.FindAll(e => e.result).Count, relevant_datapoints.FindAll(e => !e.result).Count);


        //Changing Image in Frame to current filtered for category value
        //assumes that there are matching images in 3dicons folder
        if (layer.layerLevel != 0)
        {
            string source_path = "";
            string category = filtered_for[filtered_for.Count - 1];
            source_path += category;
            source_path += "_";
            string choice = dataPoints[0].values[category];
            source_path += choice;
            GameObject icon = Resources.Load<GameObject>("3DIcons/ReworkedPrefabs/" + source_path) as GameObject;
            categorieIcon = Instantiate(icon);
            categorieIcon.transform.SetParent(transform, false);
            categorieIcon.transform.localPosition = new Vector3(0f, 0.4f, 0.5f);
        }
        

    }


    /// <summary>
    /// Activates means: Show the buttons for this frame so that the next discrimination can be chosen by the user
    /// This is called by a layer on all its frames
    /// </summary>
    public void Activate()
    { 
        child_nodes = new List<GameObject>(); 
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

        if (categories_filtered_for.Contains(DataHandler.categories[0].Value<string>("id")))
        {
            outlook_button.gameObject.SetActive(false);
        }
        if (categories_filtered_for.Contains(DataHandler.categories[1].Value<string>("id")))
        {
            temp_button.gameObject.SetActive(false);
        }
        if (categories_filtered_for.Contains(DataHandler.categories[2].Value<string>("id")))
        {
            hum_button.gameObject.SetActive(false);
        }
        if (categories_filtered_for.Contains(DataHandler.categories[3].Value<string>("id")))
        {
            wind_button.gameObject.SetActive(false);
        }

        //TODO generalize by iterating over 0-4 not temp_button etc.

        //update Entropy Buttons
        if (isEntropyFrame) gameObject.GetComponent<EntropyHandler>().updateButton(choose_button);
    }


    public void ButtonClick_Outlook()
    {
        if (this.choose_button is null) return;

        choose_button.gameObject.SetActive(false);
        Create_child_nodes(DataHandler.categories[0].Value<string>("id"));
    }

    public void ButtonClick_Temp() {
        if (this.choose_button is null) return;
        choose_button.gameObject.SetActive(false);
        Create_child_nodes(DataHandler.categories[1].Value<string>("id"));
    }

    public void ButtonClick_Humidity()
    {
        if (this.choose_button is null) return;

        choose_button.gameObject.SetActive(false);
        Create_child_nodes(DataHandler.categories[2].Value<string>("id"));
    }
    public void ButtonClick_Wind()
    {
        if (this.choose_button is null) return;

        choose_button.gameObject.SetActive(false);
        Create_child_nodes(DataHandler.categories[3].Value<string>("id"));
    }



    /// <summary>
    /// Creates the child nodes according to the chosen categorie discriminated for
    /// Gives the child nodes the necessary information to place and go on in the tree
    /// </summary>
    /// <param name="filtered">The category the datapoints have to be discriminated by</param>
    void Create_child_nodes(string filtered)
    {
        button_pressed = true;

        Layer next_layer = layer.NextLayer();
        child_nodes.Clear();

        List<string> new_filtered_for = new List<string>(categories_filtered_for);
        new_filtered_for.Add(filtered);

        int number_datapoints_to_left = layer.GetCountDPsToTheLeftForNextLayer(this);
        int added_dp_to_Left = 0;

        JToken categorie_filtered = null;
        foreach (JObject cat in DataHandler.categories)
        {
            if (cat["id"].Value<string>() == filtered) categorie_filtered = cat;
        }
        List<DataPointNew> dps;
        int ind_for_sorting = 1;
        foreach (string choice in categorie_filtered["choices"].Values<string>())
        {
            GameObject child = Instantiate(frame_prefab, transform.parent);
            if (!(layer.layerLevel == 0)) //Set Icon from Parent inactive in child
            {
                child.transform.Find(categorieIcon.name).gameObject.SetActive(false);
            }
            dps = dataPoints.FindAll(e => e.values[filtered] == choice);
            child.GetComponent<FrameHandler>()
                .InitFrame(new_filtered_for, dps, next_layer, numberForSorting * 10 + ind_for_sorting++, number_datapoints_to_left + added_dp_to_Left, color);
            added_dp_to_Left += dps.Count;
            child_nodes.Add(child);
        }

        layer.ListenerNodeGeneratesChildren();

    }

    /// <summary>
    /// Equals if numberfor sorting is equal
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public override bool Equals(System.Object o)
    {
#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
        if (this == o) return true;
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast
        if (o == null || GetType() != o.GetType()) return false;
        FrameHandler frame_handler = (FrameHandler) o;
        return frame_handler.numberForSorting == this.numberForSorting;
    }

    
}
