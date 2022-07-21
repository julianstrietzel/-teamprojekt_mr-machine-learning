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
    private Vector3 next_global_position;
    private float speed = 0.5f;

    private float buffer = .1f; //Buffer between the layers


    private void Update()
    {   
        if (move)
        {
            moved += Vector3.Distance(Vector3.forward * Time.deltaTime * speed, Vector3.zero);
             
            transform.Translate( Vector3.forward  * Time.deltaTime * speed);
            if (moved > 1f + buffer)
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
        roothandler.InitFrame(new List<dataPoint.categories>(), DataHandler.data, layerZero, 1, 0);
        place_button = Instantiate(button_prefab, gameObject.transform.parent.parent);
        UnityEngine.Events.UnityEvent button_pressed = place_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed;
        button_pressed.AddListener(Dissable_Following);
        button_pressed.AddListener(roothandler.Activate);
        place_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Placed correctly?";
    }

    public void MoveUpForNextLayer() {
        move = true;
    }
}
