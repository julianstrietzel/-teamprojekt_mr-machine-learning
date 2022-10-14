using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// For the handmenu button "reset".
/// </summary>
public class ResetScriptPOV : MonoBehaviour
{
      public void ResetScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
