using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseCanvas;
    [SerializeField]
    private GameObject gameCanvas;
    [SerializeField]
    private GameObject inventoryCanvas;

    private bool onInventory = false;
    private bool onPause = false;
    private float slowTime = 0f; 
    public bool isPausedTime { get; private set; } = false;


    public void PressExit()
    {
        if (onPause)
        {
            Continue();
        }
        else
        {
            OnPause();
        }
    }

    public void PressInventory()
    {
        if (onInventory)
        {
            HideInventory();
        }
        else
        {
            if (!onPause)
            {
                OpenInventory();
            }
        }
        isPausedTime = true;
    }

    public void Continue()
    {
        onInventory = false;
        onPause = false;
        pauseCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        inventoryCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPausedTime = false;
    }

    public void OnPause()
    {
        onPause = true;
        onInventory = false;
        pauseCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        Time.timeScale = slowTime;
        isPausedTime = true;
    }

    public void OpenInventory()
    {
        onInventory = true;
        inventoryCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        Time.timeScale = slowTime;
        isPausedTime = true;
    }

    public void HideInventory()
    {
        onInventory = false;
        pauseCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        Time.timeScale = 1f;
        isPausedTime = false;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        GameObject.Find("Main Camera/PauseCanvas/Exit/LoadingScreen").SetActive(true);
        SceneManager.LoadScene("MainMenu");
    }
}
