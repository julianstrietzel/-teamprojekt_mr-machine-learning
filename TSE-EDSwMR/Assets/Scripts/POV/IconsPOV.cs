using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Script has to be added to Icons Parent and the parent has to be added to Components GameObject in Unity.
 * 
 */
public class IconsPOV : MonoBehaviour
{
    private Vector3[] positionIcons;// = new Vector3[AMOUNT_ICONS];
    private Vector3[] scaleIcons;// = new Vector3[AMOUNT_ICONS];


    // updated after each "day" and set on the icons for the new day
    private GameObject[] currentIcons = new GameObject[StateScriptPOV.AMOUNT_ICONS];

    private List<GameObject[]> iconsList = new List<GameObject[]>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Intatiates the next icon in the correct position
    /// </summary>
    /// <param name="icon_nr"> number between 1 and 3 </param>
    public void DisplayNextIcon(int day, int icon_nr)
    {
        if (day <= StateScriptPOV.AMOUNT_DAYS && icon_nr <= StateScriptPOV.AMOUNT_ICONS)
        {
            if (icon_nr == 1 && !(currentIcons[0] is null))
            {
                foreach (GameObject icons in currentIcons)
                {
                    Destroy(icons); // or DestroyComponent?
                }
            }

            int icon_nr_array = icon_nr - 1;

            if (iconsList[day - 1][icon_nr_array] != null)
            {
                currentIcons[icon_nr_array] = GameObject.Instantiate(iconsList[day - 1][icon_nr_array], gameObject.transform);
                currentIcons[icon_nr_array].transform.localPosition = positionIcons[icon_nr_array];
                currentIcons[icon_nr_array].transform.localScale = scaleIcons[icon_nr_array];
            }
        }
    }


                                   
    public void DestroyParentAfterGameFinish()
    {
        Destroy(gameObject);
    }

    public void SetIconPositionScale(List<GameObject[]> icons, Vector3[] positions, Vector3[] scale)
    {
        iconsList = icons;
        positionIcons = positions;
        scaleIcons = scale;


    }

}
