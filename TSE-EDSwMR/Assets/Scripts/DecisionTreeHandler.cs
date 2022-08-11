using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Microsoft.MixedReality.Toolkit.UI;

public class DecisionTreeHandler : MonoBehaviour
{

    public DataHandler data;
    public GameObject button_prefab;
    public GameObject info_box;
    public GameObject frame_prefab;
    
    protected GameObject place_button;
    public static ArrayList s_layers = new ArrayList();
    public static float s_max_width = 4f;

    protected bool move;
    protected float moved = 0;
    protected float speed = 0.5f;

    protected float buffer = .1f; //Buffer between the layers
    protected static Color prev_color;
    protected static Color yellow_plate_color = new Color(255, 230, 132);
    protected static Color red_plate_color = new Color(217, 0, 69);



    public virtual void Update()
    {
        if (move)
        {
            moved += Vector3.Distance(Vector3.forward * Time.deltaTime * speed, Vector3.zero);

            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            if (moved > .3f * (1f + buffer))
            {
                move = false;
                moved = 0f;
            }
        }
    }

    public void Dissable_Following()
    {
        gameObject.transform.GetComponentInParent<Microsoft.MixedReality.Toolkit.Utilities.Solvers.SolverHandler>().enabled = false;
        place_button.SetActive(false);

    }

    public virtual void OnDataHandlerInit()
    {
        GameObject root = Instantiate(frame_prefab, gameObject.transform);
        Layer layerZero = new Layer(0, DataHandler.data.Count, null, this);
        FrameHandler roothandler = root.GetComponent<FrameHandler>();

        s_layers = new ArrayList();
        s_layers.Add(layerZero);
        prev_color = Color.blue;
        roothandler.InitFrame(new List<string>(), DataHandler.data, layerZero, 1, 0, prev_color);

        place_button = Instantiate(button_prefab, gameObject.transform.parent.parent);
        place_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Placed correctly?";

        UnityEvent button_pressed = EnableFollowing();
        button_pressed.AddListener(Dissable_Following);
        button_pressed.AddListener(roothandler.Activate);
        button_pressed.AddListener(DeactivateTooltip);


    }

    public void DeactivateTooltip()
    {
        info_box.SetActive(false);
    }

    /// <summary>
    /// This function will in every situation (after init of the DT) make the dt refollow the users gaze.
    /// Can be used to replace the tree if hololens fucks it up
    /// </summary>
    /// <returns>The UnityEvent triggered when the placement button is pressed. There you can add individual Listeners. 
    /// (Not necessary for the function but maybe handy)</returns>
    public UnityEvent EnableFollowing()
    {
        gameObject.transform.GetComponentInParent<Microsoft.MixedReality.Toolkit.Utilities.Solvers.SolverHandler>().enabled = true;
        place_button.SetActive(true);

        return place_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed;
    }

    public void MoveUpForNextLayer()
    {
        move = true;
    }

    

    public static Color RandomColor()
    {
        float threshold_similarity = 1f; //TODO calibrate level of similarity
        if (prev_color == null) return prev_color = new Color(UnityEngine.Random.Range(0, 255) / 255f, UnityEngine.Random.Range(0, 255) / 255f, UnityEngine.Random.Range(0, 255) / 255f);
        Color new_color;
        int i = 0;
        do
        {
            i++;
            new_color = new Color(UnityEngine.Random.Range(0, 255) / 255f, UnityEngine.Random.Range(0, 255) / 255f, UnityEngine.Random.Range(0, 255) / 255f);
        } while (i < 10
        && unsimilarity(prev_color, new_color) < threshold_similarity
        && unsimilarity(new_color, yellow_plate_color) < threshold_similarity
        && unsimilarity(new_color, red_plate_color) < threshold_similarity
        );
        if (i == 10) Debug.Log("No new random color found. Exit with similar color");
        return prev_color = new_color;

        double unsimilarity(Color color_a, Color color_b)
        {
            return Math.Pow(color_a.b - color_b.b, 2) + Math.Pow(color_a.r - color_b.r, 2) + Math.Pow(color_a.g - color_b.g, 2);
        }

    }
}
