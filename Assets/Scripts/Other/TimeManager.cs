using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public static class TimeManager
{
    /*public static UnityEvent<float> changeScaleGame = new UnityEvent<float>();
    public static UnityEvent<float> changeScaleUI = new UnityEvent<float>();
    public static UnityEvent<float> changeScaleCharacter = new UnityEvent<float>();

    private static float scaleGame = 1f;
    private static float scaleUI = 1f;
    private static float scaleCharacter = 1f;

    public static void SetScaleGame(float scale)
    {
        scaleGame = scale;
        changeScaleGame.Invoke(scale);
    }

    public static void SetScaleCharacter(float scale)
    {
        scaleCharacter = scale;
        changeScaleCharacter.Invoke(scale);
    }

    public static void SetScaleUI(float scale)
    {
        scaleUI = scale;
        changeScaleGame.Invoke(scale);
    }

    public static IEnumerator WaitForSecondsGame(float maxTime)
    {
        float currentTime = 0f;
        while (currentTime <= maxTime)
        {
            currentTime += Time.deltaTime * scaleGame;
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator WaitForSecondsCharacter(float maxTime)
    {
        float currentTime = 0f;
        while (currentTime <= maxTime)
        {
            currentTime += Time.deltaTime * scaleCharacter;
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator WaitForSecondsUI(float maxTime)
    {
        float currentTime = 0f;
        while (currentTime <= maxTime)
        {
            currentTime += Time.deltaTime * scaleUI;
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator WaitForEndOfFrameCharacter()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (scaleCharacter == 1)
                break;
        }
    }

    public static IEnumerator WaitForEndOfFrameUI()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (scaleUI == 1)
                break;
        }
    }

    public static IEnumerator WaitForEndOfFrameGame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (scaleGame == 1)
                break;
        }
    }*/
}
