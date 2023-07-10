using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    //
    // Screne Exxeption
    //
    private int currentException = 5;

    private string[] Exception =
    {
        "1024 x 768",
        "1280 x 720",
        "1280 x 1024",
        "1600 x 900",
        "1680 x 1050",
        "1920 x 1080",
    };

    public void ExceptionPressedLeft()
    {
        if(currentException != 0)
        {
            currentException--;
        }
        else
        {
            currentException = Exception.Length - 1;
        }
        NewException();
    }

    public void ExceptionPressedRight()
    {
        if (currentException != Exception.Length - 1)
        {
            currentException++;
        }
        else
        {
            currentException = 0;
        }
        NewException();
    }

    private void NewException()
    {
        GameObject obj = GameObject.Find("TextExtension(TMP)");
        TextMeshProUGUI textTMP = obj.GetComponent<TextMeshProUGUI>();
        textTMP.text = Exception[currentException];
    }
    
    //
    // Frame limit
    //
    private int currentFPS = 1;

    private int[] FPS =
    {
        60,
        120
    };

    public void FPSPressedLeft()
    {
        if (currentFPS != 0)
        {
            currentFPS--;
        }
        else
        {
            currentFPS = FPS.Length - 1;
        }
        NewFPS();
    }

    public void FPSPressedRight()
    {
        if (currentFPS != FPS.Length - 1)
        {
            currentFPS++;
        }
        else
        {
            currentFPS = 0;
        }
        NewFPS();
    }

    private void NewFPS()
    {
        GameObject obj = GameObject.Find("TextFPS(TMP)");
        TextMeshProUGUI textTMP = obj.GetComponent<TextMeshProUGUI>();
        textTMP.text = FPS[currentFPS].ToString();
    }

    //
    // Language
    //
    private int currentLanguage = 0 ;

    private string[] Language =
    {
        "Українська",
        "English"
    };

    public void LanguagePressedLeft()
    {
        if (currentLanguage != 0)
        {
            currentLanguage--;
        }
        else
        {
            currentLanguage = Language.Length - 1;
        }
        NewLanguage();
    }

    public void LanguagePressedRight()
    {
        if (currentLanguage != Language.Length - 1)
        {
            currentLanguage++;
        }
        else
        {
            currentLanguage = 0;
        }
        NewLanguage();
    }

    private void NewLanguage()
    {
        GameObject obj = GameObject.Find("TextLanguage(TMP)");
        TextMeshProUGUI textTMP = obj.GetComponent<TextMeshProUGUI>();
        textTMP.text = Language[currentLanguage];
    }

    //
    // V-sync
    //
    private bool syncV = false;

    public void PressedVsync()
    {
        if(syncV == true)
        {
            syncV = false;
        }
        else
        {
            syncV = true;
        }

        GameObject obj = GameObject.Find("ButtonVsync");
        Button button= obj.GetComponent<Button>();

        string spriteName;
        if (syncV == true)
        {
            spriteName = "Button3";
        }
        else
        {
            spriteName = "Button2";
        }

        Sprite sprite = Resources.Load<Sprite>("Button3");
        button.image.sprite = sprite;
    }
}
