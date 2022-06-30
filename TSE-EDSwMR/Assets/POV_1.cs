using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POV_1 : MonoBehaviour
{
    public GameObject icon1_day1;
    //public GameObject icon2_day1;
    //public GameObject icon3_day1;
    //public GameObject icon1_day2;
    //public GameObject icon2_day2;
    //public GameObject icon3_day2;
    //public GameObject icon1_day3;
    //public GameObject icon2_day3;
    //public GameObject icon3_day3;
    //public GameObject icon1_day4;
    //public GameObject icon2_day4;
    //public GameObject icon3_day4;


    public GameObject[] icons_day1_prefab = new GameObject[3];
    public GameObject[] icons_day2_prefab = new GameObject[3];
    public GameObject[] icons_day3_prefab = new GameObject[3];
    public GameObject[] icons_day4_prefab = new GameObject[3];


    private GameObject[] icons_day1 = new GameObject[3];
    private GameObject[] icons_day2 = new GameObject[3];
    private GameObject[] icons_day3 = new GameObject[3];
    private GameObject[] icons_day4 = new GameObject[3];

    private GameObject[] currentIcons;

    private static Vector3 position_icon1 = new Vector3(-0.04f, 0.04f, -0.01f);
    private static Vector3 position_icon2 = new Vector3(-0.04f, 0.0f, -0.025f);
    private static Vector3 position_icon3 = new Vector3(-0.04f, -0.04f, -0.025f);




    private GameObject icon1_d1;

    // current day in the story.
    private int day;
    private int iconNr;
    // Start is called before the first frame update
    void Start()
    {
        day = 1;

        iconNr = 1;


        // wait for user input
        // then increase iconNr and displaynext icon again until we have 3 icons.

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoopIconsAnswer()
    {
        while (iconNr <= 3)
        {
            DisplayNextIcon(iconNr);

            iconNr++;
        }
    }

    //private void DisplayIcons()
    //{
    //    // icon1 = GameObject.Instantiate(icon1_prefab, gameObject.transform);
    //    icon1_d1 = GameObject.Instantiate(icon1_day1, gameObject.transform);
    //    icon1_d1.transform.localPosition = new Vector3(-0.04f, 0.04f, -0.01f);

    //}


    /// <summary>
    /// Intatiates the next icon in the correct position
    /// </summary>
    /// <param name="icon_nr"> number between 1 and 3 </param>
    private void DisplayNextIcon(int icon_nr)
    {
        icon_nr--; // because of array
        currentIcons[icon_nr] = GameObject.Instantiate(IconsOfDay_prefabs()[icon_nr],gameObject.transform);


        if (icon_nr == 1)
        {
            currentIcons[icon_nr].transform.localPosition = position_icon1;

        }
        else if (icon_nr == 2)
        {
            currentIcons[icon_nr].transform.localPosition = position_icon2;

        }
        else if (icon_nr == 3)
        {
            currentIcons[icon_nr].transform.localPosition = position_icon3;

        }

    }

    /// <summary>
    /// Returns an Array with the icons of the current day in the story
    /// Days 1 to 4 
    /// </summary>
    /// <returns></returns>
    private GameObject[] IconsOfDay_prefabs()
    {
        if (day == 1)
        {
            return icons_day1_prefab;
        }

        else if (day == 2)
        {
            return icons_day2_prefab;
        }
        
        else if (day == 3)
        {
            return icons_day3_prefab;
        }
        else if (day == 4)
        {
            return icons_day4_prefab;
        }


        return null;
    }

    //private void SetCurrentIcons()
    //{
    //    currentIcons = IconsOfDay();

    //}
}
