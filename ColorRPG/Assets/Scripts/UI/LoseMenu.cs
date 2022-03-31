using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    public void Start()
    {
        if (UIManager.instance != null)
        {
            Destroy(UIManager.instance.gameObject);
        }
    }

    /// <summary>
    /// Changes the scene
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    public void Btn_LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
