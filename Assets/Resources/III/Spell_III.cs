using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class Spell_III : MonoBehaviour
{
    [SerializeField]
    private float reloadTime = 4f;

    public readonly bool MOMENTARYCAST = false;

    private float spellDistance = 20f;
    private Vector3 currentGamepadPosition = Vector3.zero;
    private bool isSpellReady = true;
    private Vector3 cursorSpellPosition = Vector3.zero;

    private GameObject cursorPrefabModel;
    private GameObject effectPrefabModel;
    private GameObject cursorModel;
    private List<GameObject> effectModel = new List<GameObject>(); 
    private string cursorName = "III/Cursor";
    private string effectName = "III/IceArrow";

    void Start()
    {
        cursorPrefabModel = Resources.Load<GameObject>(cursorName);
        cursorModel = Instantiate(cursorPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
        cursorModel.SetActive(false);

        effectPrefabModel = Resources.Load<GameObject>(effectName);
        for (int i = 0; i < 5; i++)
        {
            GameObject currentEffectModel = Instantiate(effectPrefabModel, new Vector3(0f, -20f, 0f), Quaternion.identity);
            currentEffectModel.SetActive(false);
            effectModel.Add(currentEffectModel);
        }
        //iii = effectModel.GetComponentInChildren<III>();
    }

    public void CastSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.SetActive(true);
        cursorModel.transform.position = DistanceWithRadius(cursorPosition, characterPosition, isGamepadUsing);
    }

    private Vector3 DistanceWithRadius(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        Vector3 pointCenterSpell = PointCenterSpell(cursorPosition, characterPosition, isGamepadUsing) - characterPosition;

        if (isGamepadUsing)
        {
            if (Mathf.Sqrt(pointCenterSpell.x * pointCenterSpell.x + pointCenterSpell.z * pointCenterSpell.z) < spellDistance)
            {
                return pointCenterSpell + characterPosition;
            }
            else
            {
                pointCenterSpell.y = 0f;
                pointCenterSpell.Normalize();

                currentGamepadPosition = pointCenterSpell * spellDistance;

                pointCenterSpell = characterPosition + currentGamepadPosition;

                return pointCenterSpell;
            }
        }
        else
        {
            float deltaX = cursorPosition.x - characterPosition.x;
            float deltaZ = cursorPosition.z - characterPosition.z;

            if (Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ) < spellDistance)
            {
                return pointCenterSpell + characterPosition;
            }
            else
            {
                Vector3 pointDirection = cursorPosition - characterPosition;
                pointDirection.y = 0f;
                pointDirection.Normalize();

                pointCenterSpell = characterPosition + pointDirection * spellDistance;

                return pointCenterSpell;
            }
        }
    }

    private Vector3 PointCenterSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        Vector3 pointCenterSpell = Vector3.zero;

        if (isGamepadUsing)
        {
            if (cursorPosition.x < 0f)
            {
                cursorPosition.x = cursorPosition.x * cursorPosition.x;
                cursorPosition.x = -cursorPosition.x;
            }
            else
            {
                cursorPosition.x = cursorPosition.x * cursorPosition.x;
            }

            if (cursorPosition.z < 0f)
            {
                cursorPosition.z = cursorPosition.z * cursorPosition.z;
                cursorPosition.z = -cursorPosition.z;
            }
            else
            {
                cursorPosition.z = cursorPosition.z * cursorPosition.z;
            }

            currentGamepadPosition += cursorPosition;

            pointCenterSpell = characterPosition + currentGamepadPosition * Time.deltaTime;
        }
        else
        {
            pointCenterSpell = cursorPosition;
        }

        return pointCenterSpell;
    }

    public void CancelSpell(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        cursorModel.transform.position += new Vector3(0f, -20f, 0f);
    }

    public void CastSpellEnd(Vector3 cursorPosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        StartCoroutine(Reload());

        cursorSpellPosition = cursorModel.transform.position;

        cursorModel.transform.position += new Vector3(0f, -20f, 0f);
        cursorModel.SetActive(false);

        StartCoroutine(EffectCast(characterPosition));
    }

    IEnumerator EffectCast(Vector3 characterPosition)
    {
        Vector3 pointDirection = cursorSpellPosition - characterPosition;

        float distanceEffect = Mathf.Sqrt(pointDirection.x * pointDirection.x + pointDirection.z * pointDirection.z);
        distanceEffect += 2f;

        float currentDistance = 0f;
        while(currentDistance < distanceEffect)
        {
            effectModel[0].transform.position = 10f * pointDirection.normalized * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public bool IsSpellReady()
    {
        return isSpellReady;
    }

    IEnumerator Reload()
    {
        isSpellReady = false;
        yield return new WaitForSeconds(reloadTime);
        isSpellReady = true;
    }
}
