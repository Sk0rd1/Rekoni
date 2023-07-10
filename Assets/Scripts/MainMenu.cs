using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        Debug.Log("—цену запущено.");
        SceneManager.LoadScene("TestScene2");
    }

    public void ExitGame()
    {
        Debug.Log("¬их≥д з гри.");
        Application.Quit();
    }
}
