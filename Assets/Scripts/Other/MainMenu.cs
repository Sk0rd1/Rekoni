using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingScreen;

    private void Start()
    {

        loadingScreen.SetActive(false);
    }

    public void LoadGame()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("TestScene2");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
