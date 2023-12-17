using System;
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

    public void PressExit()
    {
        if (onInventory && !onPause)
        {
            HideInventory();
        }
        else if (onPause)
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
    }

    public void Continue()
    {
        onInventory = false;
        onPause = false;
        pauseCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        inventoryCanvas.SetActive(false);
        GameObject.Find("Main Camera").GetComponent<SpellsInfo>().FillSpellManager();
    }

    public void OnPause()
    {
        onPause = true;
        onInventory = false;
        pauseCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
    }

    public void OpenInventory()
    {
        onInventory = true;
        inventoryCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        GameObject.Find("Main Camera").GetComponent<SpellsInfo>().NewInfoInInventory();
    }

    public void HideInventory()
    {
        onInventory = false;
        pauseCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        GameObject.Find("Main Camera").GetComponent<SpellsInfo>().FillSpellManager();
    }

    public void Exit()
    {
        GameObject.Find("Main Camera/PauseCanvas/Exit/LoadingScreen").SetActive(true);
        SceneManager.LoadScene("MainMenu");
    }
}
