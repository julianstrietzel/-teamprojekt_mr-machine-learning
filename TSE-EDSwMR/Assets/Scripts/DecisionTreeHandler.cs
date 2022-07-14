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


    // Start is called before the first frame update
    void Start()
    {
        

        
        

        //TODO place the root frame where appropriete according to scene understanding
    }


    private void Dissable_Following()
    {
        print("Dissable working");
        gameObject.transform.GetComponentInParent<Microsoft.MixedReality.Toolkit.Utilities.Solvers.SolverHandler>().enabled = false;
        place_button.SetActive(false);
    }

    public  void OnDataHandlerInit()
    {
        GameObject root = Instantiate(frame_prefab, gameObject.transform);
        s_layers = new ArrayList();
        Layer layerZero = new Layer(0, DataHandler.data.Count, null);
        s_layers.Add(layerZero);

        root.GetComponent<FrameHandler>()
            .InitFrame(new List<dataPoint.categories>(), DataHandler.data, layerZero, 1, 0);
        place_button = Instantiate(button_prefab, gameObject.transform.parent.parent);
        place_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed.AddListener(Dissable_Following);
        place_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Placed correctly?";
    }
}
