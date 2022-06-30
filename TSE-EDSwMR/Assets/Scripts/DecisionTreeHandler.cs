using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTreeHandler : MonoBehaviour
{

    public DataHandler data;
    public GameObject button_prefab;

    public GameObject frame_prefab;
    private GameObject place_button;
    // Start is called before the first frame update
    void Start()
    {
        
        GameObject root = Instantiate(frame_prefab, gameObject.transform);

        root.GetComponent<FrameHandler>()
            .InitFrame(new List<dataPoint.categories>(), DataHandler.data, 0, -2, +2);
        place_button = Instantiate(button_prefab, gameObject.transform.parent.parent);
        place_button.GetComponent<Microsoft.MixedReality.Toolkit.UI.PressableButtonHoloLens2>().ButtonPressed.AddListener(Dissable_Following);
        place_button.transform.GetChild(2).transform.GetChild(0).transform.GetComponent<TMPro.TextMeshPro>().text = "Placed correctly?";

        //TODO place the root frame where appropriete according to scene understanding
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Dissable_Following()
    {
        print("Dissable working");
        gameObject.transform.GetComponentInParent<Microsoft.MixedReality.Toolkit.Utilities.Solvers.SolverHandler>().enabled = false;
        place_button.SetActive(false);
    }
}
