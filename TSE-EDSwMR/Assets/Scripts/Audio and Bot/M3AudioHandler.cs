using UnityEngine;

/// <summary>
/// This class calls the correct Audiofiles on the M3 (Rebuild Decisiontree)
/// </summary>
public class M3AudioHandler : MonoBehaviour
{



    public void PlayIntro()
    {
        print("M3PlayIntroduction");
    }

    public void PlaySumUp()
    {

        print("M3PlaySUmUp");
    }

    /// <summary>
    /// Is played when the first layer is built, to give an extre thought
    /// </summary>
    public void PlayAdditionalNotes()
    {
        print("M3PlayAdditionalNotes");

    }

}
