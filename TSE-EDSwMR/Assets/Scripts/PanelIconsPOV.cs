using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelIconsPOV : MonoBehaviour
{

    // TODO try to not hard code
    public static int AMOUNT_ICONS = 3; // is not acctually needed in Unity
    public static int AMOUNT_DAYS = 4; // is not acctually needed in Unity




    private static readonly Vector3 position_icon1 = new Vector3(-0.04f, 0.04f, -0.01f);
    private static readonly Vector3 position_icon2 = new Vector3(-0.04f, 0.0f, -0.025f);
    private static readonly Vector3 position_icon3 = new Vector3(-0.04f, -0.04f, -0.025f);

    //private static readonly Vector3 position_icon_test = new Vector3(-0.4f, 0.4f, -0.1f);


    //public Vector3[] positionIcons = new Vector3[AMOUNT_ICONS]; // does not work




    //public Vector3 PositionClass
    //{
    //    get { return position_icon1; }
    //    set
    //    {
    //        if (position_icon1 != value)
    //        {
    //            position_icon1 = value;
    //        }
    //    }
    //}


    public GameObject[] iconsDay11Prefab;


    [SerializeField] GameObject[] iconsDay1Prefab = new GameObject[AMOUNT_ICONS];
    public GameObject[] iconsDay2Prefab = new GameObject[AMOUNT_ICONS];
    public GameObject[] iconsDay3Prefab = new GameObject[AMOUNT_ICONS];
    public GameObject[] iconsDay4Prefab = new GameObject[AMOUNT_ICONS];



    private GameObject[] icons_day1;
    private GameObject[] icons_day2;
    private GameObject[] icons_day3;
    private GameObject[] icons_day4;

    private GameObject[] currentIcons = new GameObject[AMOUNT_ICONS];

    //private Vector3[] position_icons;







    //private GameObject icon1_d1;

    // current day in the story.
    //private int day;
    //private int icon_nr;
    // Start is called before the first frame update
    void Start()
    {
        //currentIcons = iconsDay1Prefab;
        //position_icons = positionIcons;

        //Debug.Log(iconsDay1Prefab.Length + " iconsDay1Prefab.Length");

        //Debug.Log(position_icon1 + " position Icon 1 ");

        ////Debug.Log(PositionClass + " PositionClass");

        //Debug.Log(iconsDay1Prefab[0] + " iconsDay1Prefab[0] ");

        //Debug.Log(iconsDay1Prefab[1] + " iconsDay1Prefab[1] ");

        //Debug.Log(iconsDay2Prefab[0] + " iconsDay2Prefab[0] ");

       




        ////iconsDay11Prefab = new GameObject[AMOUNT_ICONS];
        //DisplayIcons();

        

       

        //for (int i = 0; i < icons_day1_prefab.Length; i++)
        //{
        //    currentIcons[i] = GameObject.Instantiate(IconsOfDay_prefabs()[i], gameObject.transform);

        //}


        // LoopIconsAnswer();

        // wait for user input
        // then increase iconNr and displaynext icon again until we have 3 icons.

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    //private void LoopIconsAnswer()
    //{
    //    // changing global variable really?
    //    while (icon_nr <= 3)
    //    {
    //        DisplayNextIcon();

    //        icon_nr++;
    //    }
    //}



    public void DisplayIcons()
    {
        //Debug.Log(position_icon1 + " position Icon 1- Display Icons");
        //Debug.Log(position_icon2 + " position Icon 2 - Display Icons");
        Debug.Log(position_icon_test + " position TEST");


        Debug.Log(iconsDay1Prefab[0] + " iconsDay1Prefab[0]");


        Debug.Log("display icons");

        icons_day1[0] = GameObject.Instantiate(iconsDay1Prefab[0], gameObject.transform);
        icons_day1[0].transform.localPosition = position_icon1;
        //// icon1 = GameObject.Instantiate(icon1_prefab, gameObject.transform);
        //icon1_d1 = GameObject.Instantiate(icon1_day1, gameObject.transform);
        //icon1_d1.transform.localPosition = new Vector3(-0.04f, 0.04f, -0.01f);
        icons_day1[1] = GameObject.Instantiate(iconsDay1Prefab[1], gameObject.transform);

        icons_day1[1].transform.localPosition = position_icon2;

        





    }


    /// <summary>
    /// Intatiates the next icon in the correct position
    /// </summary>
    /// <param name="icon_nr"> number between 1 and 3 </param>
    public void DisplayNextIcon(int day, int icon_nr)
    {

        Debug.Log("display NEXT ICON");

        // TODO destroy icons after
        if (icon_nr == 1 && currentIcons[0] != null)
        {

        }

        //if (day > 1)
        //{
        //    foreach (GameObject icons in currentIcons)
        //    {
        //        Destroy(icons); // or DestroyComponent?
        //    }
        //}
        int icon_nr_array = icon_nr - 1;

        Debug.Log(gameObject.transform + " PARENT"); // is Component GameObject - that is wrong 

        Debug.Log(GetCurrentIcons_prefabs(day)[icon_nr_array] + " GetCurrentIcons_prefabs(day)[icon_nr_array]");

        // TODO Change gameobject.transform to the Panel
        currentIcons[icon_nr_array] = GameObject.Instantiate(GetCurrentIcons_prefabs(day)[icon_nr_array],gameObject.transform);


        if (icon_nr == 1)
        {
            currentIcons[0].transform.localPosition = position_icon1;

        }
        else if (icon_nr == 2)
        {
            currentIcons[1].transform.localPosition = position_icon2;

        }
        else if (icon_nr == 3)
        {
            currentIcons[2].transform.localPosition = position_icon3;

        }
        //int i = 0;
        //while (i < 2)
        //{
        //    DisplayNextIcon(day, icon_nr + 1);
        //    i++;
        //}

    }

    //public static void NextIconSwitch(int day)
    //{
    //    switch (day)
    //    {
    //        case < 1.0:
    //            break;

    //    }
    //}

    /// <summary>
    /// Returns an Array with the icons of the current day in the story
    /// Days 1 to 4 
    /// </summary>
    /// <returns></returns>
    private GameObject[] GetCurrentIcons_prefabs(int day)
    {
        if (day == 1)
        {
            return iconsDay1Prefab;
        }

        else if (day == 2)
        {
            return iconsDay2Prefab;
        }
        
        else if (day == 3)
        {
            return iconsDay3Prefab;
        }
        else if (day == 4)
        {
            return iconsDay4Prefab;
        }


        return null;
    }

    //private void SetCurrentIcons()
    //{
    //    currentIcons = IconsOfDay();

    //}

}
