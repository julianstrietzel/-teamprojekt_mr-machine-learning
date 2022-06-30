using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POV_1 : MonoBehaviour
{
    public GameObject icon1_prefab;
    private GameObject icon1;
    // Start is called before the first frame update
    void Start()
    {
        //  icon1 = test.getComponent<HotIcon>;

        //icon1.transform.position = new Vector3(0.04f, 0.04f, 0.025f);
         DisplayIcons();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayIcons()
    {
        // icon1 = GameObject.Instantiate(icon1_prefab, gameObject.transform);
        icon1 = GameObject.Instantiate(icon1_prefab, gameObject.transform);
        icon1.transform.localPosition = new Vector3(-0.04f, 0.04f, -0.025f);

    }
}
