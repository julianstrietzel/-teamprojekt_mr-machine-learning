using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScriptPOV : MonoBehaviour
{
      public void ResetScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
