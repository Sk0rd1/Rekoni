using RobinGoodfellow.CircleGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSM_Spell : SpellUniversal
{
    public const bool MOMENTARYCAST = true;
    private float force = 10f;
    private float damage = 10f;
    private float reloadTime = 0.1f;

    private bool isSpellReady = true;
    private string effectName = "SSM/BombBlack";
    private GameObject effectModel;
    private Vector3 shieldOffset = new Vector3(0f, 2.9f, 0f);
    private float currentReload = 0f;
    GameObject characterGirl;
    private float bombSpeed = 10f;

    private void Start()
    {
        effectModel = Resources.Load<GameObject>(effectName);
        characterGirl = GameObject.Find("CharacterGirl");
    }

    public override bool IsMomemtaryCast()
    {
        return MOMENTARYCAST;
    }

    public override bool IsSpellReady()
    {
        return isSpellReady;
    }

    public override float TimeReload()
    {
        return currentReload;
    }

    public override void FirstStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {

    }

    public override void SecondStageOfCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {
        StartCoroutine(Reload());
        StartCoroutine(BombEffect());
    }

    public override void CancelCast(Vector3 mousePosition, Vector3 characterPosition, bool isGamepadUsing)
    {

    }

    IEnumerator BombEffect()
    {
        GameObject bomb = Instantiate(effectModel, characterGirl.transform.position + new Vector3(0, -1f, 0), Quaternion.identity);

        Vector3 oldPosition = characterGirl.transform.position;

        yield return new WaitForEndOfFrame();

        Vector3 bombDirection = oldPosition - characterGirl.transform.position;
        bombDirection.Normalize(); 

        while (bomb.transform.position.y < 10f)
        {
            bomb.transform.position += (bombDirection + new Vector3(0, 4, 0)) * bombSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (bomb.transform.position.y > 4.75f)
        {
            bomb.transform.position += (bombDirection + new Vector3(0, -4, 0)) * bombSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        bomb.transform.position = new Vector3(bomb.transform.position.x, 5f, bomb.transform.position.z);

        SSM ssm = bomb.GetComponent<SSM>();
        ssm.SetValues((int)damage);
    }

    IEnumerator Reload()
    {
        isSpellReady = false;
        currentReload = reloadTime;
        while (currentReload >= 0f)
        {
            currentReload -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isSpellReady = true;
    }
}
