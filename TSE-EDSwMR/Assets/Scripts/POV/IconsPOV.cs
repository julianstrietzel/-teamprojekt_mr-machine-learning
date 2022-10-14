using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Initialized the icons. Handels the correct order, scale and position, information is handed over by <see cref="StateScriptPOV"/>
/// Script has to be added to Icons Parent and the parent has to be added to Components GameObject in Unity.
/// </summary>
public class IconsPOV : MonoBehaviour
{
    private Vector3[] positionIcons;
    private Vector3[] scaleIcons;


    // updated after each "day" and set on the icons for the new day
    private GameObject[] currentIcons = new GameObject[StateScriptPOV.AMOUNT_ICONS];
    // all icons in correct order
    private List<GameObject[]> iconsList = new List<GameObject[]>();

    // sets the given (GameObject) icons, position of the icons and the scale, gets the information from state script, where it is set in unity inspector.
    public void SetIconPositionScale(List<GameObject[]> icons, Vector3[] positions, Vector3[] scale)
    {
        iconsList = icons;
        positionIcons = positions;
        scaleIcons = scale;


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
                    Destroy(icons);
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

}
