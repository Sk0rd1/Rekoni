using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System;

public class SaveManager
{
    string filePath = Application.persistentDataPath + "/Rekoni.save";
    Rune[] runesArray;

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Create);

        Save save = new Save();

        ExpendableResources expRes = GameObject.Find("CharacterGirl").GetComponent<ExpendableResources>();

        save.expendable = new Expendable(expRes.NumCoins, expRes.SphereM, expRes.SphereP, expRes.SphereS, expRes.SphereU, expRes.SphereI);

        int[] spellsNum = GameObject.Find("Main Camera").GetComponent<SpellsInfo>().GetValues();

        save.spellsNum = spellsNum;

        bf.Serialize(fs, save);

        fs.Close();
    }

    public void LoadGame()
    {
        if (!File.Exists(filePath))
            return ;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream( filePath, FileMode.Open);

        Save save = (Save)bf.Deserialize(fs);
        fs.Close();

        GameObject.Find("CharacterGirl").GetComponent<ExpendableResources>().SetValues(save.expendable.numCoins, save.expendable.sphereM, save.expendable.sphereP, save.expendable.sphereS, save.expendable.sphereU, save.expendable.sphereI);
        GameObject.Find("Main Camera").GetComponent<SpellsInfo>().SetValues(save.spellsNum);
    }
}

[Serializable]
public class Save
{
    public Expendable expendable;

    public int[] spellsNum = new int[8];

    //Rune[] runesArray;
}

[Serializable]
public struct Expendable
{
    public int numCoins;
    public int sphereM;
    public int sphereP;
    public int sphereS;
    public int sphereU;
    public int sphereI;

    public Expendable(int numCoins, int sphereM, int sphereP, int sphereS, int sphereU, int sphereI)
    {
        this.numCoins = numCoins;
        this.sphereM = sphereM;
        this.sphereP = sphereP;
        this.sphereS = sphereS;
        this.sphereU = sphereU;
        this.sphereI = sphereI;
    }
}

[Serializable]
public struct Rune
{
    public int timeOfDamage;

    public float reloadTime;
    public float damage;
    public float slow;
    public float poisonDamage;
    public float fieDamage;
    public float percentDamage;
    public float specialEffect;
    public float castDuration;
    public float stunDuration;
    public float heal;
    public float shield;

    public Rune(int timeOfDamage, float reloadTime = 0f, float damage = 0f, float slow = 0f, float poisonDamage = 0f, float fieDamage = 0f, float percentDamage = 0f, float specialEffect = 0f, float castDuration = 0f, float stunDuration = 0f, float heal = 0f, float shield = 0f)
    {
        this.timeOfDamage = timeOfDamage;
        this.reloadTime = reloadTime;
        this.damage = damage;
        this.slow = slow;
        this.poisonDamage = poisonDamage;
        this.fieDamage = fieDamage;
        this.percentDamage = percentDamage;
        this.specialEffect = specialEffect;
        this.castDuration = castDuration;
        this.stunDuration = stunDuration;
        this.heal = heal;
        this.shield = shield;
    }
}
