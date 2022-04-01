using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject controlsMenu;

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Btn_QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Loads the game scene
    /// </summary>
    public void Btn_StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        if (UIManager.instance != null)
        {
            UIManager.instance.townScene = true;
            //UIManager.instance.townMenuRef.SetActive(true);
        }
    }

    public void Btn_ToggleControls()
    {
        controlsMenu.SetActive(!controlsMenu.activeSelf);
    }
}
