using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTreeHandler : MonoBehaviour
{

    public DataHandler data;
    public GameObject button_prefab;

    public GameObject frame_prefab;
    private GameObject place_button;
    public static ArrayList s_layers = new ArrayList();
    public static float s_max_width = 2;

    private bool move;
    private float moved = 0;
    private float speed = 0.5f;

    private float buffer = .1f; //Buffer between the layers
    private static Color prev_color;

    private void Update()
    {   
        if (move)
        {
            moved += Vector3.Distance(Vector3.forward * Time.deltaTime * speed, Vector3.zero);
             
            transform.Translate( Vector3.forward  * Time.deltaTime * speed);
            if (moved > .3f * (1f + buffer))
            {
                move = false;
                moved = 0f;
            }
        }
    }

    private void Dissable_Following()
    {
        //print("Dissable working");
        gameObject.transform.GetComponentInParent<Microsoft.MixedReality.Toolkit.Utilities.Solvers.SolverHandler>().enabled = false;
        place_button.SetActive(false);


    }

    public  void OnDataHandlerInit()
    {
        GameObject root = Instantiate(frame_prefab, gameObject.transform);
        s_layers = new ArrayList();
        Layer layerZero = new Layer(0, DataHandler.data.Count, null, this);
        s_layers.Add(layerZero);
        FrameHandler roothandler = root.GetComponent<FrameHandler>();
        prev_color = Color.blue; 
        roothandler.InitFrame(new List<dataPoint.categories>(), DataHandler.data, layerZero, 1, 0, prev_color);
        place_button = Instantiate(button_prefab, gameObject.transform.parent.parent);
        UnityEngine.Events.UnityEvent button_pressed = place_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed;
        button_pressed.AddListener(Dissable_Following);
        button_pressed.AddListener(roothandler.Activate);
        place_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Placed correctly?";
    }

    public void MoveUpForNextLayer() {
        move = true;
    }

    public static Color RandomColor()
    {
        float level_similarity = .3f; //TODO calibrate level of similarity
        print("prevColor is " + prev_color);
        if (prev_color == null) return prev_color = new Color(Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f);
        Color new_color;
        int i = 0;
        do {
            i++;
            new_color = new Color(Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f);            
        } while (i < 5 && (prev_color.b - new_color.b) * (prev_color.b - new_color.b) + (prev_color.r - new_color.r) * (prev_color.r - new_color.r) + (prev_color.b - new_color.b) * (prev_color.b - new_color.b) < level_similarity);
        //TODO make color not to similar to red and yellow of the plates
        return prev_color = new_color;

    }
}
