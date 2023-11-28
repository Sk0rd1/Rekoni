using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    //Примітка. Якщо обєкт на який поміщено клас SetActive(false)
    //то таймер продовжується на тому ж місці де зупинився
    //timeForOneFrame і typeOfAnimation не скидуються
    //0 - Game 1 - Character 2 - UI  
    [SerializeField]
    private Sprite[] sprites;

    float timeForOneFrame = 1;

    int typeOfAnimation = -1;

    private void OnEnable()
    {
        if (typeOfAnimation != -1)
        {
            if (gameObject.GetComponent<Image>().sprite != sprites[sprites.Length - 1])
            {
                int spriteIndex = Array.IndexOf(sprites, gameObject.GetComponent<Image>().sprite);
                Debug.Log(spriteIndex);
                if (typeOfAnimation == 0)
                    StartCoroutine(GameAnimation(spriteIndex));
                else if (typeOfAnimation == 1)
                    StartCoroutine(CharacterAnimation(spriteIndex));
                else if (typeOfAnimation == 2)
                    StartCoroutine(UIAnimation(spriteIndex));
            }
        }
    }

    public void StartCharacterAnimation(float timeForOneFrame)
    {
        this.timeForOneFrame = timeForOneFrame;
        typeOfAnimation = 1;
        StopAllCoroutines();
        StartCoroutine(CharacterAnimation());
    }

    public void StartGameAnimation(float timeForOneFrame)
    {
        typeOfAnimation = 0;
        this.timeForOneFrame = timeForOneFrame;
        StopAllCoroutines();
        StartCoroutine(GameAnimation());
    }

    public void StartUIAnimation(float timeForOneFrame)
    {
        typeOfAnimation = 2;
        this.timeForOneFrame = timeForOneFrame;
        StopAllCoroutines();
        StartCoroutine(UIAnimation());
    }

    private IEnumerator CharacterAnimation(int numOfFrame = 0)
    {
        Color color = gameObject.GetComponent<Image>().color;
        color.a = 1;
        gameObject.GetComponent<Image>().color = color;

        for (int i = numOfFrame; i < sprites.Length; i++)
        {
            gameObject.GetComponent<Image>().sprite = sprites[i];
            yield return new WaitForSeconds(timeForOneFrame);
        }

        color.a = 0;
        gameObject.GetComponent<Image>().color = color;
    }

    private IEnumerator GameAnimation(int numOfFrame = 0)
    {
        Color color = gameObject.GetComponent<Image>().color;
        color.a = 1;
        gameObject.GetComponent<Image>().color = color;

        for (int i = numOfFrame; i < sprites.Length; i++)
        {
            gameObject.GetComponent<Image>().sprite = sprites[i];
            yield return new WaitForSeconds(timeForOneFrame);
        }

        color.a = 0;
        gameObject.GetComponent<Image>().color = color;
    }

    private IEnumerator UIAnimation(int numOfFrame = 0)
    {
        Color color = gameObject.GetComponent<Image>().color;
        color.a = 1;
        gameObject.GetComponent<Image>().color = color;

        for (int i = numOfFrame; i < sprites.Length; i++)
        {
            gameObject.GetComponent<Image>().sprite = sprites[i];
            yield return new WaitForSeconds(timeForOneFrame);
        }

        color.a = 0;
        gameObject.GetComponent<Image>().color = color;
    }
}
